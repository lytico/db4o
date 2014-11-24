package db4ounit;

import java.lang.reflect.*;

import com.db4o.foundation.*;

/**
 * @sharpen.ignore
 */
public class UnitTestMain {
	
	public static void main(String[] args) throws Exception {
		new UnitTestMain().runTests(args);
	}

	public final void runTests(String[] args) throws ClassNotFoundException,
			InstantiationException, IllegalAccessException {
		new ConsoleTestRunner(build(args), false).run();
	}
	
	protected Iterable4 builder(Class clazz) {
		return new ReflectionTestSuiteBuilder(clazz);
	}

	private Iterable4 build(String[] args)
			throws ClassNotFoundException, InstantiationException,
			IllegalAccessException {
		
		return Iterators.concatMap(Iterators.iterable(args), new Function4() {
			public Object apply(Object arg) {
				String testIdentifier = (String)arg;
				try {
					int methodSeparatorIndex = testIdentifier.indexOf('#');
					if (methodSeparatorIndex>0) {
						String className=testIdentifier.substring(0,methodSeparatorIndex);
						String methodName=testIdentifier.substring(methodSeparatorIndex+1);
						return Iterators.singletonIterable(testMethod(className, methodName));
					}
					return builder(Class.forName(testIdentifier));
				} catch (Exception x) {
					return new FailingTest(testIdentifier, x);
				}
			}
		});
	}
	
	private Test testMethod(String className, String methodName)
			throws ClassNotFoundException, InstantiationException,
			IllegalAccessException {
		Class clazz = Class.forName(className);
		final Test test = wrapTest(new TestMethod(clazz.newInstance(), findMethod(clazz, methodName)));
		if(!ClassLevelFixtureTest.class.isAssignableFrom(clazz)) {
			return test;
		}
		return new ClassLevelFixtureTestSuite(clazz, new Closure4<Iterator4<Test>>() {
			public Iterator4<Test> run() {
				return Iterators.iterate(test);
			}
		});
	}
	
	protected Test wrapTest(Test test) {
		return test;
	}

	private Method findMethod(final Class clazz, String methodName) {
		Method[] methods = clazz.getMethods();
		for (int i = 0; i < methods.length; i++) {
			Method method = methods[i];
			if (method.getName().equals(methodName)) {
				return method;
			}
		}
		throw new IllegalArgumentException("Method '" + methodName + "' not found in class '" + clazz + "'.");
	}
}
