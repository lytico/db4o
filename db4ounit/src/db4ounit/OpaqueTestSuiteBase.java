package db4ounit;

import com.db4o.foundation.*;

public abstract class OpaqueTestSuiteBase implements Test {

	private Closure4<Iterator4<Test>> _tests;
	
	public OpaqueTestSuiteBase(Closure4<Iterator4<Test>> tests) {
		_tests = tests;
	}

	public void run() {
		TestExecutor executor = Environments.my(TestExecutor.class);
		Iterator4<Test> tests = _tests.run();
		try {
			suiteSetUp();
			while(tests.moveNext()) {
				executor.execute(tests.current());
			}
			suiteTearDown();
		}
		catch(Exception exc) {
			executor.fail(this, exc);
		}
	}
	
	public boolean isLeafTest() {
		return false;
	}
	
	protected Closure4<Iterator4<Test>> tests() {
		return _tests;
	}
	
	public Test transmogrify(final Function4<Test, Test> fun) {
		return transmogrified(
			new Closure4<Iterator4<Test>>() {
				public Iterator4<Test> run() {
					return Iterators.map(tests().run(), new Function4<Test, Test>() {
						public Test apply(Test test) {
							return fun.apply(test);
						}
					});
				}
			});
	}

	protected abstract OpaqueTestSuiteBase transmogrified(Closure4<Iterator4<Test>> tests);
	
	protected abstract void suiteSetUp() throws Exception;
	protected abstract void suiteTearDown() throws Exception;

}
