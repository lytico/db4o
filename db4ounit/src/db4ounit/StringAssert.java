/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package db4ounit;

public class StringAssert {
	
	public static void contains(String expected, String actual) {
		if(actual.indexOf(expected) >= 0){
			return;
		}
		Assert.fail("'" + actual + "' does not contain '" + expected + "'");
	}
	
}
