/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.foundation.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
final class ReflectArrayIterator extends IndexedIterator {
    
    private final Object _array;
    
    private final ReflectArray _reflectArray;

    public ReflectArrayIterator(ReflectArray reflectArray, Object array) {
        super(reflectArray.getLength(array));
        _reflectArray = reflectArray;
        _array = array;
    }

    protected Object get(int index) {
        return _reflectArray.get(_array, index);
    }
}