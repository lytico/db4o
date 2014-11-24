/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.reflect;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TestReflectConstructor {
	
	// adjust, in case you add further test constructors
	public static final int CONSTRUCTOR_COUNT = 2;
	
	public TestReflectConstructor(String str, int i){
		// this one is called.
		// don't modify
		// the test is looking for the String constructor
		// and uses a String and an int to create an instance
	}
	
	public TestReflectConstructor(int i){
		
	}
}

