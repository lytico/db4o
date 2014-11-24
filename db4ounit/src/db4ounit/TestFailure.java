package db4ounit;

import java.io.IOException;
import java.io.Writer;

public class TestFailure extends Printable {
	
	private final String _testLabel;
	private final Throwable _failure;
	
	public TestFailure(String test, Throwable failure) {
		_testLabel = test;
		_failure = failure;
	}
	
	/**
	 * @sharpen.property
	 */
	public String testLabel() {
		return _testLabel;
	}
	
	/**
	 * @sharpen.property
	 */
	public Throwable reason() {
		return _failure;
	}
	
	public void print(Writer writer) throws IOException {
		writer.write(_testLabel);
		writer.write(": ");
		// TODO: don't print the first stack trace elements
		// which reference db4ounit.Assert methods
		TestPlatform.printStackTrace(writer, _failure);
	}
}
