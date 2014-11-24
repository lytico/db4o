/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions;

public abstract class ComposibleTestSuite extends Db4oTestSuite {

	protected final Class[] composeTests(Class[] testCases) {
		return concat(testCases, composeWith());
	}

	
	protected Class[] composeWith() {
		return new Class[0];
	}

	public static Class[] concat(Class[] testCases, Class[] otherTests) {		
		Class[] result = new Class[otherTests.length + testCases.length];
		System.arraycopy(testCases, 0, result, 0, testCases.length);
		System.arraycopy(otherTests, 0, result, testCases.length, otherTests.length);
		
		return result;
	}
}
