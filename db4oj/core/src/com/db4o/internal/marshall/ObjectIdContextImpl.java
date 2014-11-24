/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.marshall.*;

/**
 * @exclude
 */
public class ObjectIdContextImpl extends ObjectHeaderContext implements ObjectIdContext {
    
    private final int _id;

    public ObjectIdContextImpl(Transaction transaction, ReadBuffer buffer, ObjectHeader objectHeader, int id) {
        super(transaction, buffer, objectHeader);
        _id = id;
    }
    
    public int objectId(){
        return _id;
    }

}
