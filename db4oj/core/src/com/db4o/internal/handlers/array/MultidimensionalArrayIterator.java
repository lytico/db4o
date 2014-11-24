/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.foundation.*;
import com.db4o.reflect.*;


/**
 * @exclude
 */
public class MultidimensionalArrayIterator implements Iterator4 {
    
    private final ReflectArray _reflectArray;
    
    private final Object[] _array;
    
    private int _currentElement;
    
    private Iterator4 _delegate;
    
    public MultidimensionalArrayIterator(ReflectArray reflectArray, Object[] array) {
        _reflectArray = reflectArray;
        _array = array;
        reset();
    }

    public Object current() {
        if(_delegate == null){
            return _array[_currentElement];
        }
        return _delegate.current();
    }

    public boolean moveNext() {
        if(_delegate != null){
            if(_delegate.moveNext()){
                return true;
            }
            _delegate = null;
        }
        _currentElement++;
        if(_currentElement >= _array.length){
            return false;
        }
        Object obj = _array[_currentElement];
        Class clazz = obj.getClass();
        if(clazz.isArray()){
            if(clazz.getComponentType().isArray()){
                _delegate = new MultidimensionalArrayIterator(_reflectArray, (Object[]) obj);
            } else {
                _delegate = new ReflectArrayIterator(_reflectArray, obj);
            }
            return moveNext();
        }
        return true;
    }

    public void reset() {
        _currentElement = -1;
        _delegate = null;
    }

}
