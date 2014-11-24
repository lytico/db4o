package db4ounit.tests;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {
	
	protected Class[] testCases() {
		return new Class[] {
			AssertTestCase.class,
			ClassLevelFixtureTestTestCase.class,
			CompositeTestListenerTestCase.class,
			ExceptionInTearDownDoesNotShadowTestCase.class,
			FrameworkTestCase.class,
			OpaqueTestSuiteFailureTestCase.class,
			OpaqueTestSuiteTestCase.class,
			ReflectionTestSuiteBuilderTestCase.class,
			ReinstantiatePerMethodTest.class,
			TestExceptionWithInnerCause.class,
			TestLifeCycleTestCase.class,
			TestRunnerTestCase.class,
			
			db4ounit.tests.data.AllTests.class,
			db4ounit.tests.fixtures.AllTests.class,
		};
	}
		
	public static void main(String[] args) {
		new ConsoleTestRunner(AllTests.class).run();
	}	
}
