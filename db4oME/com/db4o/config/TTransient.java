/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.config;

import com.db4o.*;

/**
 * @exclude
 */
public class TTransient implements ObjectConstructor {
    
    public Object onStore(ObjectContainer con, Object object){
        return null;
    }

    public void onActivate(ObjectContainer con, Object object, Object members){
    }

    public Class storedClass(){
        return Object.class;
    }

    public Object onInstantiate(ObjectContainer container, Object storedObject) {
        return null;
    }
}
