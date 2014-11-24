package db4ounit.tests;

import com.db4o.foundation.*;

import db4ounit.*;

public class OpaqueTestSuiteFailureTestCase implements TestCase {

	public void testFailOnSetup() {
		BooleanByRef tearDownCalled = new BooleanByRef();
		TestResult result = new TestResult();
		new TestRunner(Iterators.iterable(new FailingTestSuite(true, false, tearDownCalled))).run(result);
		Assert.areEqual(0, result.testCount());
		Assert.areEqual(1, result.failures().size());
		Assert.isFalse(tearDownCalled.value);
	}

	public void testFailOnTearDown() {
		BooleanByRef tearDownCalled = new BooleanByRef();
		TestResult result = new TestResult();
		new TestRunner(Iterators.iterable(new FailingTestSuite(false, true, tearDownCalled))).run(result);
		Assert.areEqual(1, result.testCount());
		Assert.areEqual(2, result.failures().size());
		Assert.isTrue(tearDownCalled.value);
	}

	public static class FailingTestSuite extends OpaqueTestSuiteBase {

		private boolean _failOnSetUp;
		private boolean _failOnTeardown;
		private BooleanByRef _tearDownCalled;
		
		public FailingTestSuite(boolean failOnSetup, boolean failOnTeardown, BooleanByRef tearDownCalled) {
			this(failOnSetup, failOnTeardown, tearDownCalled, new Closure4<Iterator4<Test>>() {
				public Iterator4<Test> run() {
					return Iterators.iterate(new FailingTest("fail", new AssertionException("fail")));
				}
			});
		}

		private FailingTestSuite(boolean failOnSetup, boolean failOnTeardown, BooleanByRef tearDownCalled, Closure4<Iterator4<Test>> tests) {
			super(tests);
			_failOnSetUp = failOnSetup;
			_failOnTeardown = failOnTeardown;
			_tearDownCalled = tearDownCalled;
		}

		@Override
		protected void suiteSetUp() throws Exception {
			if(_failOnSetUp) {
				Assert.fail();
			}
		}

		@Override
		protected void suiteTearDown() throws Exception {
			_tearDownCalled.value = true;
			if(_failOnTeardown) {
				Assert.fail();
			}
		}

		@Override
		protected OpaqueTestSuiteBase transmogrified(Closure4<Iterator4<Test>> tests) {
			return new FailingTestSuite(_failOnSetUp, _failOnTeardown, _tearDownCalled, tests);
		}

		public String label() {
			return getClass().getName();
		}
		
	}
	
}
