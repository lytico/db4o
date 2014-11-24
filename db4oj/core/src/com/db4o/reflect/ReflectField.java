/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect;

/** 
 * representation for java.lang.reflect.Field.
 * <br><br>See the respective documentation in the JDK API.
 * @see Reflector
 */
public interface ReflectField {
	
	public Object get(Object onObject);
	
	public String getName();
	
	/**
	 * The ReflectClass returned by this method should have been
	 * provided by the parent reflector.
	 * 
	 * @return the ReflectClass representing the field type as provided by the parent reflector
	 */
	public ReflectClass getFieldType();
	
	public boolean isPublic();
	
	public boolean isStatic();
	
	public boolean isTransient();
	
	public void set(Object onObject, Object value);
	
	/**
	 * The ReflectClass returned by this method should have been
	 * provided by the parent reflector.
	 * 
	 * @return the ReflectClass representing the index type as provided by the parent reflector
	 */
	public ReflectClass indexType();
	
	public Object indexEntry(Object orig);
}
