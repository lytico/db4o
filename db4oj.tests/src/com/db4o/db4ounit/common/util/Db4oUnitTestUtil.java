/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.util;

public class Db4oUnitTestUtil {

	public static Class[] mergeClasses(Class[] classesLeft, Class[] classesRight) {
		if(classesLeft == null || classesLeft.length == 0) {
			return classesRight;
		}
		if(classesRight == null || classesRight.length == 0) {
			return classesLeft;
		}
		Class[] merged = new Class[classesLeft.length + classesRight.length];
		System.arraycopy(classesLeft, 0, merged, 0, classesLeft.length);
		System.arraycopy(classesRight, 0, merged, classesLeft.length, classesRight.length);
		return merged;
	}
	
	private Db4oUnitTestUtil() {
	}
}
