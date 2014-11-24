/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.reflect;

/** 
 * representation for java.lang.reflect.Field.
 * <br><br>See the respective documentation in the JDK API.
 * @see Reflector
 */
public interface ReflectField {
	
	public Object get(Object onObject);
	
	public String getName();
	
	public ReflectClass getType();
	
	public boolean isPublic();
	
	public boolean isStatic();
	
	public boolean isTransient();
	
	public void set(Object onObject, Object value);
	
	public void setAccessible();  //TODO Refactoring suggestion: Let the set(onObject, value) method take care of this.
	
}
