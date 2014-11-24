/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;



/**
 * @exclude
 */
public interface ObjectIdContext extends HandlerVersionContext, InternalReadContext {
    
    public int objectId();

}
