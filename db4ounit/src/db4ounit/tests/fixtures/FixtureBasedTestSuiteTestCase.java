/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.tests.fixtures;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.fixtures.*;
import db4ounit.mocking.*;

public class FixtureBasedTestSuiteTestCase implements TestCase {
	
	static FixtureVariable<MethodCallRecorder> RECORDER_FIXTURE = FixtureVariable.newInstance("recorder");
	
	static FixtureVariable FIXTURE1 = new FixtureVariable("f1");
	
	static FixtureVariable FIXTURE2 = new FixtureVariable("f2");
	
	public static final class TestUnit implements TestCase {
		
		private final Object fixture1 = FIXTURE1.value();
		private final Object fixture2 = FIXTURE2.value();
		
		public void testFoo() {
			record("testFoo");
		}
		
		public void testBar() {
			record("testBar");
		}

		private void record(final String test) {
			recorder().record(new MethodCall(test, fixture1, fixture2));
		}

		private MethodCallRecorder recorder() {
			return RECORDER_FIXTURE.value();
		}
	}
	
	public void test() {
		
		final MethodCallRecorder recorder = new MethodCallRecorder();
		
		run(new FixtureBasedTestSuite() {
			public FixtureProvider[] fixtureProviders() {
				return new FixtureProvider[] {
					new SimpleFixtureProvider(RECORDER_FIXTURE, recorder),
					new SimpleFixtureProvider(FIXTURE1, "f11", "f12"),
					new SimpleFixtureProvider(FIXTURE2, "f21", "f22"),
				};
			}

			public Class[] testUnits() {
				return new Class[] { TestUnit.class };
			}
		});
		
		
//		System.out.println(CodeGenerator.generateMethodCallArray(recorder));
		
		recorder.verify(new MethodCall[] {
			new MethodCall("testFoo", "f11", "f21"),
			new MethodCall("testFoo", "f11", "f22"),
			new MethodCall("testFoo", "f12", "f21"),
			new MethodCall("testFoo", "f12", "f22"),
			new MethodCall("testBar", "f11", "f21"),
			new MethodCall("testBar", "f11", "f22"),
			new MethodCall("testBar", "f12", "f21"),
			new MethodCall("testBar", "f12", "f22")
		});
	}
	
	public void testCombinationToRun() {
		
		final MethodCallRecorder recorder = new MethodCallRecorder();
		
		run(new FixtureBasedTestSuite() {
			public FixtureProvider[] fixtureProviders() {
				return new FixtureProvider[] {
					new SimpleFixtureProvider(RECORDER_FIXTURE,  recorder ),
					new SimpleFixtureProvider(FIXTURE1,  "f11", "f12" ),
					new SimpleFixtureProvider(FIXTURE2,  "f21", "f22" ),
				};
			}

			public Class[] testUnits() {
				return new Class[] { TestUnit.class };
			}
			
			public int[] combinationToRun() {
				return new int[] { 0, 0, 1 };
			}
		});
		
		
//		System.out.println(CodeGenerator.generateMethodCallArray(recorder));
		
		recorder.verify(new MethodCall[] {
			new MethodCall("testFoo", "f11", "f22"),
			new MethodCall("testBar", "f11", "f22"),
		});
	}
	
	public void testInvalidCombinationToRun() {
		
		Assert.expect(AssertionException.class, new CodeBlock() {
			public void run() {
				runInvalidCombination();
			}
		});
	}
	
	private void runInvalidCombination() {
		run(new FixtureBasedTestSuite() {
			public FixtureProvider[] fixtureProviders() {
				return new FixtureProvider[] {
						new SimpleFixtureProvider(FIXTURE1,  "f11", "f12" ),
						new SimpleFixtureProvider(FIXTURE2,  "f21", "f22" ),
				};
			}
			
			public Class[] testUnits() {
				return new Class[] { TestUnit.class };
			}
			
			public int[] combinationToRun() {
				return new int[] { 0 };
			}
		});
	}

	private void run(final FixtureBasedTestSuite suite) {
		final TestResult result = new TestResult();
		new TestRunner(suite).run(result);
		if (result.failures().size() > 0) {
			Assert.fail(Iterators.toString(result.failures()));
		}
	}
	
	public void testLabel() {
		final FixtureBasedTestSuite suite = new FixtureBasedTestSuite() {
			public FixtureProvider[] fixtureProviders() {
				return new FixtureProvider[] {
					new SimpleFixtureProvider(FIXTURE1,  "f11", "f12" ),
					new SimpleFixtureProvider(FIXTURE2,  "f21", "f22" ),
				};
			}

			public Class[] testUnits() {
				return new Class[] { TestUnit.class };
			}
		};
		final Iterable4 labels = Iterators.map(suite, new Function4() {
			public Object apply(Object arg) {
				return ((Test)arg).label();
			}
		});
		Iterator4Assert.areEqual(new Object[] {
			testLabel("testFoo", 0, 0),
			testLabel("testFoo", 1, 0),
			testLabel("testFoo", 0, 1),
			testLabel("testFoo", 1, 1),
			testLabel("testBar", 0, 0),
			testLabel("testBar", 1, 0),
			testLabel("testBar", 0, 1),
			testLabel("testBar", 1, 1)
		}, labels.iterator());
	}

	private String testLabel(final String testMethod, int fixture1Index, int fixture2Index) {
		final String prefix = "(f2[" + fixture1Index + "]) (f1[" + fixture2Index + "]) ";
		return prefix + TestUnit.class.getName() + "." + testMethod;
	}


}
