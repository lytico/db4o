/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * Useful as "out" or "by reference" function parameter.
 */
public class ByRef<T> {

	public static <T> ByRef<T> newInstance(T initialValue) {
		final ByRef<T> instance = new ByRef<T>();
		instance.value = initialValue;
		return instance;
	}
	
	public static <T> ByRef<T> newInstance() {
		return new ByRef<T>();
	}    
    public T value;
}
