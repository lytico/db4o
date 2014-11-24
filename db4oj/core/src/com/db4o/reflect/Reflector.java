/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect;

import com.db4o.foundation.DeepClone;

/**
 * root of the reflection implementation API.
 * <br><br>The open reflection interface is supplied to allow to implement
 * reflection functionality on JDKs that do not come with the
 * java.lang.reflect.* package.<br><br>
 * Use {@link com.db4o.config.CommonConfiguration#reflectWith configuration.commmon().reflectWith(IReflect reflector)}
 * to register the use of your implementation before opening database
 * files.
 */
public interface Reflector extends DeepClone{
	
	void configuration(ReflectorConfiguration config);
	
	/**
	 * returns an ReflectArray object, the equivalent to java.lang.reflect.Array.
	 */
	public ReflectArray array();
	
	/**
	 * returns an ReflectClass for a Class
	 */
	public ReflectClass forClass(Class clazz);
	
	/**
	 * returns an ReflectClass class reflector for a class name or null
	 * if no such class is found
	 */
	public ReflectClass forName(String className);
	
	/**
	 * returns an ReflectClass for an object or null if the passed object is null.
	 */
	public ReflectClass forObject(Object obj);
	
	public boolean isCollection(ReflectClass clazz);
    
    public void setParent(Reflector reflector);
	
}
