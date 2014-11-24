/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.types;

/**
 * base interface for db4o collections
 */
public interface Db4oCollection extends Db4oType{
    
    /**
     * configures the activation depth for objects returned from this collection.
     * <br><br>Specify a value less than zero to use the default activation depth
     * configured for the ObjectContainer or for individual objects.
     * @param depth the desired depth
     */
    public void activationDepth(int depth);
    
    
    /**
     * configures objects are to be deleted from the database file if they are
     * removed from this collection.
     * <br><br>Default value: <code>false</code>
     * @param flag the desired setting
     */
    public void deleteRemoved(boolean flag);
    

}
