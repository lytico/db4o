/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect;

import com.db4o.internal.*;

/** 
 * representation for java.lang.reflect.Method.
 * <br><br>See the respective documentation in the JDK API.
 * @see Reflector
 */
public interface ReflectMethod {
	
	public Object invoke(Object onObject, Object[] parameters) throws ReflectException;
    
    public ReflectClass getReturnType();
	
}
