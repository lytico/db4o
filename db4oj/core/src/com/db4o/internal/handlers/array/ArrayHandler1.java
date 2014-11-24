/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.marshall.*;

/**
 * @exclude
 */
public class ArrayHandler1 extends ArrayHandler3 {
	
    protected boolean handleAsByteArray(BufferContext context){
    	return false;
    }
    
    protected boolean handleAsByteArray(Object obj) {
    	return false;
    }

}
