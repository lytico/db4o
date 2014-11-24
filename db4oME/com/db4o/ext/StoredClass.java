/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

/**
 * the internal representation of a stored class.
 */
public interface StoredClass
{
    
	/**
	 * returns the name of this stored class.
	 */
	public String getName();
	
	
	/**
	 * returns an array of IDs of all stored object instances of this stored class.
	 */
	public long[] getIDs();
	
	/**
	 * returns the StoredClass for the parent of the class, this StoredClass represents.    
	 */
	public StoredClass getParentStoredClass();
	
	/**
	 * returns all stored fields of this stored class.
	 */
	public StoredField[] getStoredFields();
	
	/**
	 * renames this stored class.
	 * <br><br>After renaming one or multiple classes the ObjectContainer has
	 * to be closed and reopened to allow internal caches to be refreshed.
	 * <br><br>.NET: As the name you should provide [Classname, Assemblyname]<br><br>
	 * @param name the new name
	 */
	public void rename(String name);
	
	// TODO: add field creation
	
	/**
	 * returns an existing stored field of this stored class. 
	 * @param name the name of the field
	 * @param type the type of the field. 
	 * There are four possibilities how to supply the type:<br>
     * - a Class object.  (.NET: a Type object)<br>
     * - a fully qualified classname.<br>
     * - any object to be used as a template.<br><br>
     * - null, if the first found field should be returned.
	 * @return the {@link StoredField}
	 */
	public StoredField storedField(String name, Object type);
	 
}
