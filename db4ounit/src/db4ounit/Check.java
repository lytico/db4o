/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit;

/**
 * Utility class to enable the reuse of object comparison and checking
 * methods without asserting.
 */
public class Check {

	public static boolean objectsAreEqual(Object expected, Object actual) {
		return expected == actual
			|| (expected != null
				&& actual != null
				&& expected.equals(actual));
	}
	
	public static boolean arraysAreEqual(Object[] expected, Object[] actual) {
		if (expected == actual) return true;
		if (expected == null || actual == null) return false;
		if (expected.length != actual.length) return false;
	    for (int i = 0; i < expected.length; i++) {
	        if (!objectsAreEqual(expected[i], actual[i])) return false;
	    }
	    return true;
	}

}
