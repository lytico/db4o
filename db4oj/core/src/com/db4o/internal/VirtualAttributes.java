/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.ext.*;
import com.db4o.foundation.*;



/**
 * @exclude
 */
public class VirtualAttributes implements ShallowClone{
    
    public Db4oDatabase i_database;
    
    @Deprecated
    public long i_version;
    
    // FIXME: should be named "uuidLongPart" or even better "creationTime" 
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
