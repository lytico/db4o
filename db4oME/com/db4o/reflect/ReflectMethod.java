/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.reflect;

/** 
 * representation for java.lang.reflect.Method.
 * <br><br>See the respective documentation in the JDK API.
 * @see Reflector
 */
public interface ReflectMethod {
	
	public Object invoke(Object onObject, Object[] parameters);
    
    public ReflectClass getReturnType();
	
}
