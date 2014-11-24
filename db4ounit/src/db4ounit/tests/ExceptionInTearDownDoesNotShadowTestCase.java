package db4ounit.tests;

import com.db4o.foundation.*;

import db4ounit.*;

public class ExceptionInTearDownDoesNotShadowTestCase implements TestCase {

	public static final String IN_TEST_MESSAGE = "in test";
	public static final String IN_TEARDOWN_MESSAGE = "in teardown";

	
	public static class RunsWithExceptions implements TestLifeCycle {
		public void setUp() {
		}
		
		public void tearDown() {
			throw new RuntimeException(IN_TEARDOWN_MESSAGE);
		}
		
		public void testMethod() throws Exception {
			throw FrameworkTestCase.EXCEPTION;
		}
	}

	public static class RunsWithExceptionInTearDown implements TestLifeCycle {
		public void setUp() {
		}
		
		public void tearDown() {
			throw FrameworkTestCase.EXCEPTION;
		}
		
		public void testMethod() throws Exception {
		}
	}

	public void testExceptions() {
		final Iterator4 tests = new ReflectionTestSuiteBuilder(RunsWithExceptions.class).iterator();
		final Test test = (Test)Iterators.next(tests);
		FrameworkTestCase.runTestAndExpect(test, 1);
	}

	public void testExceptionInTearDown() {
		final Iterator4 tests = new ReflectionTestSuiteBuilder(RunsWithExceptionInTearDown.class).iterator();
		final Test test = (Test)Iterators.next(tests);
		FrameworkTestCase.runTestAndExpect(test, 1);
	}
}
