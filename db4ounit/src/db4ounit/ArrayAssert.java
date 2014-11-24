/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit;

import com.db4o.foundation.*;

/**
 * @sharpen.partial
 */
public class ArrayAssert {
	
	public static void contains(long[] array, long expected) {
		if (-1 != indexOf(array, expected)) {
			return;
		}
		Assert.fail("Expecting '" + expected + "'.");
	}
	
    public static void containsByIdentity(Object[] array, Object[] expected){
        for (int i = 0; i < expected.length; i++) {
            if (-1 == Arrays4.indexOfIdentity(array, expected[i])) {
                Assert.fail("Expecting contains '" + expected[i] + "'.");
            }
        }
    }

    public static void containsByEquality(Object[] array, Object[] expected){
        for (int i = 0; i < expected.length; i++) {
            if (-1 == Arrays4.indexOfEquals(array, expected[i])) {
                Assert.fail("Expecting contains '" + expected[i] + "'.");
            }
        }
    }

    public static void areEqual(Object[] expected, Object[] actual) {
		areEqualImpl(expected, actual);
	}
    
    public static void areEqual(String[] expected, String[] actual) {
    	// JDK 1.1 needs the conversion
		areEqualImpl(stringArrayToObjectArray(expected), stringArrayToObjectArray(actual));
	}

	private static Object[] stringArrayToObjectArray(String[] expected) {
		Object[] expectedAsObject = new Object[expected.length];
    	System.arraycopy(expected, 0, expectedAsObject, 0, expected.length);
    	return expectedAsObject;
	}

	/**
	 * @sharpen.ignore
	 */
	private static void areEqualImpl(Object[] expected, Object[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
		Assert.areSame(expected.getClass(), actual.getClass());
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}
	
	private static String indexMessage(int i) {
		return "expected[" + i + "]";
	}

	public static void areEqual(byte[] expected, byte[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}

	public static void areNotEqual(byte[] expected, byte[] actual) {
		Assert.areNotSame(expected, actual);		
		for (int i = 0; i < expected.length; i++) {
	        if (expected[i] != actual[i]) return;
	    }
		Assert.isTrue(false);
	}

	public static void areEqual(int[] expected, int[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}
	
	public static void areEqual(long[] expected, long[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}
	
	public static void areEqual(float[] expected, float[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}

	public static void areEqual(double[] expected, double[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}

	public static void areEqual(char[] expected, char[] actual) {
		if (expected == actual) return;
		if (expected == null || actual == null) Assert.areSame(expected, actual);
		Assert.areEqual(expected.length, actual.length);
	    for (int i = 0; i < expected.length; i++) {
	        Assert.areEqual(expected[i], actual[i], indexMessage(i));
	    }
	}
	
	private static int indexOf(long[] array, long expected) {
		for (int i = 0; i < array.length; ++i) {				
			if (expected == array[i]) {
				return i;
			}
		}
		return -1;
	}
}
