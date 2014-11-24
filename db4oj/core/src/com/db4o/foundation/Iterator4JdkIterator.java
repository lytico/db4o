/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import java.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public final class Iterator4JdkIterator implements Iterator{
    
    private final Iterator4 _delegate;
    
    private Object _current;
    
    public Iterator4JdkIterator(Iterator4 i){
        _delegate = i;
        if(_delegate.moveNext()){
        	_current = _delegate.current();
        }
    }

    public final boolean hasNext() {
        return _current != null;
    }

    public final Object next() {
        if (_current == null){
            throw new NoSuchElementException();
        }
        final Object result = _current;
        if(_delegate.moveNext()){
            _current = _delegate.current();
        }else{
            _current = null;
        }
        return result;
    }

    public void remove() {
        throw new UnsupportedOperationException(); 
    }
    
}
