/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.reflect;

import com.db4o.foundation.*;

/**
 * root of the reflection implementation API.
 * <br><br>The open reflection interface is supplied to allow to implement
 * reflection functionality on JDKs that do not come with the
 * java.lang.reflect.* package.<br><br>
 * See the code in com.db4o.samples.reflect for a reference implementation
 * that uses java.lang.reflect.*.
 * <br><br>
 * Use {@link com.db4o.config.Configuration#reflectWith Db4o.configure().reflectWith(IReflect reflector)}
 * to register the use of your implementation before opening database
 * files.
 */
public interface Reflector extends DeepClone{
	
	
	/**
	 * returns an IArray object, the equivalent to java.lang.reflect.Array.
	 */
	public ReflectArray array();
	
	/**
	 * specifiy whether parameterized Constructors are supported.
	 * <br><br>The support of Constructors is optional. If Constructors
	 * are not supported, every persistent class needs a public default
	 * constructor with zero parameters.
	 */
	public boolean constructorCallsSupported();
	
	/**
	 * returns an IClass for a Class
	 */
	public ReflectClass forClass(Class clazz);
	
	/**
	 * returns an IClass class reflector for a class name or null
	 * if no such class is found
	 */
	public ReflectClass forName(String className);
	
	/**
	 * returns an IClass for an object or null if the passed object is null.
	 */
	public ReflectClass forObject(Object a_object);
	
	public boolean isCollection(ReflectClass claxx);
    
    public void setParent(Reflector reflector);
	
}
