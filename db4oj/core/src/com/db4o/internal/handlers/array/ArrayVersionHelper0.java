/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;


/**
 * @exclude
 */
public class ArrayVersionHelper0 extends ArrayVersionHelper3 {
    
    public boolean isPreVersion0Format(int elementCount) {
        return elementCount >= 0;
    }

}
