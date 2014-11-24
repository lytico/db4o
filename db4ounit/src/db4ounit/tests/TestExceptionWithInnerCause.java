package db4ounit.tests;

import db4ounit.Assert;
import db4ounit.TestCase;
import db4ounit.TestException;

public class TestExceptionWithInnerCause implements TestCase {
	public void testDetailerMessage() {
		final String message = "Detailed message";
		final TestException e = new TestException(message, new Exception("The reason!"));
		Assert.isGreaterOrEqual(0, e.toString().indexOf(message));		
	}
}
