/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit;

import com.db4o.foundation.*;

public class TestRunner {
	
	public static DynamicVariable<TestExecutor> EXECUTOR = DynamicVariable.newInstance();
	
	private final Iterable4 _tests;

	public TestRunner(Iterable4 tests) {
		_tests = tests;
	}

	public void run(final TestListener listener) {
		listener.runStarted();
		TestExecutor executor = new TestExecutor() {
			public void execute(Test test) {
				runTest(test, listener);
			}

			public void fail(Test test, Throwable failure) {
				listener.testFailed(test, failure);
			}
		};
		Environments.runWith(Environments.newClosedEnvironment(executor), new Runnable() {
			public void run() {
				final Iterator4 iterator = _tests.iterator();
				while (iterator.moveNext()) {
					runTest((Test)iterator.current(), listener);
				}
			}
		});
		listener.runFinished();
	}

	private void runTest(final Test test, TestListener listener) {
		if(test.isLeafTest()) {
			listener.testStarted(test);
		}
		try {
			test.run();
		} catch (TestException x) {
		    Throwable reason = x.getReason();
			listener.testFailed(test, reason == null ? x : reason);
		} catch (Exception failure) {
			listener.testFailed(test, failure);
		}
	}

}
