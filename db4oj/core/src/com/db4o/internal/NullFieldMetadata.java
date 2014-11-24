/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.internal.marshall.*;

/**
 * @exclude
 */
public class NullFieldMetadata extends FieldMetadata {
    
    public NullFieldMetadata(){
        super(null);
    }
    
    /**
     * @param obj
     */
    public PreparedComparison prepareComparison(Object obj) {
    	return Null.INSTANCE;
    }
	
	public final Object read(ObjectIdContext context) {
	    return null;
	}
	
}
