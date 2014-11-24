/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.config;

/**
 * generic interface to allow returning an attribute of an object.
 */
public interface ObjectAttribute {
    
    /**
     * generic method to return an attribute of a parent object.
     * @param parent the parent object
     * @return Object - the attribute
     */
    public Object attribute(Object parent);

}
