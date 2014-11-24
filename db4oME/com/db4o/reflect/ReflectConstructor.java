/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.reflect;

/** 
 * representation for java.lang.reflect.Constructor.
 * <br><br>See the respective documentation in the JDK API.
 * @see Reflector
 */
public interface ReflectConstructor {
	
	public void setAccessible();
	
	public ReflectClass[] getParameterTypes();
	
	public Object newInstance(Object[] parameters);
	
}

