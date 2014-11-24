/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.marshall.*;


/**
 * @exclude
 */
public interface ReadsObjectIds {
    
    public ObjectID readObjectID(InternalReadContext context);

}
