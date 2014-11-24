package db4ounit.tests;

import com.db4o.foundation.*;

import db4ounit.*;

public class TestLifeCycleTestCase implements TestCase {
	public void testLifeCycle() {
		
		final ByRef<Boolean> tearDownCalled = ByRef.newInstance(false);
		RunsLifeCycle._tearDownCalled.with(tearDownCalled, new Runnable() {
			public void run() {
				final Iterator4 tests = new ReflectionTestSuiteBuilder(RunsLifeCycle.class).iterator();
				final Test test = (Test)Iterators.next(tests);
				FrameworkTestCase.runTestAndExpect(test, 1);
			}
		});
		Assert.isTrue(tearDownCalled.value);
	}
}
