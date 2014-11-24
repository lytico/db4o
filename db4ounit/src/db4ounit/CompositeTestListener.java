/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit;

public class CompositeTestListener implements TestListener {

	private final TestListener _listener1;
	private final TestListener _listener2;

	public CompositeTestListener(TestListener listener1, TestListener listener2) {
		_listener1 = listener1;
		_listener2 = listener2;
	}

	public void runFinished() {
		_listener1.runFinished();
		_listener2.runFinished();
	}

	public void runStarted() {
		_listener1.runStarted();
		_listener2.runStarted();
	}

	public void testFailed(Test test, Throwable failure) {
		_listener1.testFailed(test, failure);
		_listener2.testFailed(test, failure);
	}

	public void testStarted(Test test) {
		_listener1.testStarted(test);
		_listener2.testStarted(test);
	}

	public void failure(String msg, Throwable failure) {
		_listener1.failure(msg, failure);
		_listener2.failure(msg, failure);
	}

}
