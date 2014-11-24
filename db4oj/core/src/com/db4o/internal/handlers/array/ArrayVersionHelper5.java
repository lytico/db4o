/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.reflect.*;

/**
 * @exclude
 */
public class ArrayVersionHelper5 extends ArrayVersionHelper{
	
    public boolean hasNullBitmap(ArrayInfo info) {
        return ! info.primitive();
    }


}
