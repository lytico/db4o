/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.ext;


/**
 * generic callback interface.
 */
public interface Db4oCallback {
    
    /**
     * the callback method
     * @param obj the object passed to the callback method
     */
    public void callback(Object obj);

}
