/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;

import com.db4o.marshall.*;


/**
 * @exclude
 */
public class IntHandler0 extends IntHandler {

	@Override
    public Object read(ReadContext context) {
        int i = context.readInt();
        if (i == Integer.MAX_VALUE) {
            return null;
        }
        return new Integer(i);
    }
    
}
