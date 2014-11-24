/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import java.util.*;

/**
 * 
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
class JdkCollectionIterator4 implements Iterator4{
    
    private static final Object INVALID = new Object();
    
    private final Collection _collection;
    
    private Iterator _iterator;
    
    private Object _current;
    
    public JdkCollectionIterator4(Collection collection) {
        _collection = collection;
        reset();
    }

    public Object current() {
        if(_current == INVALID){
            throw new IllegalStateException();
        }
        return _current;
    }

    public boolean moveNext() {
        if(_iterator.hasNext()){
            _current = _iterator.next();
            return true;
        }
        _current = INVALID;
        return false;
    }

    public void reset() {
        _iterator = _collection.iterator();
        _current = INVALID; 
    }

}