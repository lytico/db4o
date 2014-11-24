/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.internal.handlers.*;


/**
 * @exclude
 */
public class IDHandler extends IntHandler {

    public void defragIndexEntry(DefragmentContextImpl context) {
    	int sourceId = context.copyIDReturnOriginalID(true);
    	context.currentParentSourceID(sourceId);
    }

}
