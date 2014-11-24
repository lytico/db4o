/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.ext.Db4oDatabase;
import com.db4o.foundation.ShallowClone;


/**
 * @exclude
 */
public class VirtualAttributes implements ShallowClone{
    
    public Db4oDatabase i_database;
    
    public long i_version;
    
    public long i_uuid;
    
    public Object shallowClone() {
    	VirtualAttributes va=new VirtualAttributes();
    	va.i_database=i_database;
    	va.i_version=i_version;
    	va.i_uuid=i_uuid;
    	return va;
    }
    
    boolean suppliesUUID(){
        return i_database != null && i_uuid != 0;
    }

}
