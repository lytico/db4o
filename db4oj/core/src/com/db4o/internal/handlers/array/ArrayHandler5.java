/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

/**
 * @exclude
 */
public class ArrayHandler5 extends ArrayHandler {
	
    protected ArrayVersionHelper createVersionHelper() {
        return new ArrayVersionHelper5();
    }

}
