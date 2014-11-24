package db4ounit;

import java.io.IOException;
import java.io.Writer;

import db4ounit.util.StopWatch;

public class TestResult extends Printable implements TestListener {

	private TestFailureCollection _failures = new TestFailureCollection();
	
	private int _testCount = 0;
	
	private final StopWatch _watch = new StopWatch();
	
	public TestResult() {
	}

	public void testStarted(Test test) {	
		++_testCount;
	}	
	
	public void testFailed(Test test, Throwable failure) {
		_failures.add(new TestFailure(test.label(), failure));
	}
	
	public void failure(String msg, Throwable failure) {
	}
	
	/**
	 * @sharpen.property
	 */
	public int testCount() {
		return _testCount;
	}

	/**
	 * @sharpen.property
	 */
	public boolean green() {
		return _failures.size() == 0;
	}

	/**
	 * @sharpen.property
	 */
	public TestFailureCollection failures() {
		return _failures;
	}
	
	public void print(Writer writer) throws IOException {		
		if (green()) {
			writer.write("GREEN (" + _testCount + " tests) - " + elapsedString() + TestPlatform.NEW_LINE);
			return;
		}
		writer.write("RED (" + _failures.size() +" out of " + _testCount + " tests failed) - " + elapsedString() + TestPlatform.NEW_LINE);				
		_failures.print(writer);
	}
	
	private String elapsedString() {
		return _watch.toString();
	}

	public void runStarted() {
		_watch.start();
	}

	public void runFinished() {
		_watch.stop();
	}

}
