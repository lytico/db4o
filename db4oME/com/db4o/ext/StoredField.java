/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.reflect.*;


/**
 * the internal representation of a field on a stored class.
 */
public interface StoredField {
    
	/**
	 * returns the field value on the passed object.
	 * <br><br>This method will also work, if the field is not present in the current
	 * version of the class.
	 * <br><br>It is recommended to use this method for refactoring purposes, if fields
	 * are removed and the field values need to be copied to other fields. 
	 */
	public Object get(Object onObject);
	
	
	/**
	 * returns the name of the field.
	 */
	public String getName();
	
	
	/**
	 * returns the Class (Java) / Type (.NET) of the field.
	 * <br><br>For array fields this method will return the type of the array.
	 * Use {link #isArray()} to detect arrays.  
	 */
	public ReflectClass getStoredType();
	
	
	/**
	 * returns true if the field is an array.
	 */
	public boolean isArray();
	
	/**
	 * modifies the name of this stored field.
	 * <br><br>After renaming one or multiple fields the ObjectContainer has
	 * to be closed and reopened to allow internal caches to be refreshed.<br><br>
	 * @param name the new name
	 */
	public void rename(String name);
	

//  will need for replication. Requested for 3.0 
//	
//	/**
//	 * sets the field value on the passed object.
//	 * @param onObject
//	 * @param fieldValue
//	 */
//	public void set(Object onObject, Object fieldValue);
}
