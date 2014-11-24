/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.tests;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.mocking.*;

public class TestRunnerTestCase implements TestCase { 
	
	static final RuntimeException FAILURE_EXCEPTION = new RuntimeException();
	
	public void testRun() {
		final RunsGreen greenTest = new RunsGreen();
		final RunsRed redTest = new RunsRed(FAILURE_EXCEPTION);
		final Iterable4 tests = Iterators.iterable(new Object[] {
			greenTest,
			redTest,
		});
		
		final MethodCallRecorder recorder = new MethodCallRecorder();
		final TestListener listener = new TestListener() {
			
			public void testStarted(Test test) {
				recorder.record(new MethodCall("testStarted", test));
			}
		
			public void testFailed(Test test, Throwable failure) {
				recorder.record(new MethodCall("testFailed", test, failure));
			}
		
			public void runStarted() {
				recorder.record(new MethodCall("runStarted"));
			}
		
			public void runFinished() {
				recorder.record(new MethodCall("runFinished"));
			}

			public void failure(String msg, Throwable failure) {
				recorder.record(new MethodCall("failure", msg, failure));
			}
			
		};
		new TestRunner(tests).run(listener);
		
		recorder.verify(new MethodCall[] {
			new MethodCall("runStarted"),
			new MethodCall("testStarted", greenTest),
			new MethodCall("testStarted", redTest),
			new MethodCall("testFailed", redTest, FAILURE_EXCEPTION),
			new MethodCall("runFinished"),
		});
	}
	
	public void testRunWithException() {
	    Test test = new Test() {

            public String label() {
                return "Test"; //$NON-NLS-1$
            }

            public void run() {
                Assert.areEqual(0, 1);
            }

			public boolean isLeafTest() {
				return true;
			}
	        
			public Test transmogrify(Function4<Test, Test> fun) {
				return fun.apply(this);
			}
	    };
	    
	    Iterable4 tests = Iterators.iterable(new Object[] {
	            test,
	    });
	    final TestResult result = new TestResult();
		new TestRunner(tests).run(result);
		Assert.areEqual(1, result.failures().size());
	}

}
