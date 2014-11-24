/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.reflect;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TestReflectClass {
	
	public static final int FIELD_COUNT = 8;
	public static final int METHOD_COUNT = 2;
	
	public String myString;
	
	public int myInt;
	
	public TestReflectClass myTyped;
	
	public Object myUntyped;
	
	public static Object myStatic;
	
	public transient Object myTransient;
	
	public Object foo(TestReflectClass paramTest){
		if(paramTest != null){
			return "OK";
		}
		throw new NullPointerException("No valid Parameter passed to TestReflectClass.foo()");
	}
	
	public void bar(){
		
	}
}

