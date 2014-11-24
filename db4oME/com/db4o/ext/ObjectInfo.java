/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;


/**
 * interface to the internal reference that an ObjectContainer
 * holds for a stored object.
 */
public interface ObjectInfo {
    
    /**
     * returns the object that is referenced.
     * <br><br>This method may return null, if the object has
     * been garbage collected.
     * @return the referenced object or null, if the object has
     * been garbage collected.
     */
    public Object getObject();
    
    /**
     * returns a UUID representation of the referenced object.
	 * UUID generation has to be turned on, in order to be able
	 * to use this feature:
	 * {@link com.db4o.config.Configuration#generateUUIDs(int)}
     * @return the UUID of the referenced object.
     */
    public Db4oUUID getUUID();
	
	/**
	 * returns the transaction serial number ("version") the 
	 * referenced object was stored with last.
	 * Version number generation has to be turned on, in order to
	 * be able to use this feature: 
	 * {@link com.db4o.config.Configuration#generateVersionNumbers(int)}
	 * @return the version number.
	 */
	public long getVersion();
	
    
    
    

}
