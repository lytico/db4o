/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public class GenericArray {
	GenericClass _clazz;
	Object[] _data;
	
    public GenericArray(GenericClass clazz, int size){
    	_clazz=clazz;
    	_data=new Object[size];
    }
    
    
    public Iterator4 iterator() {
    	return Iterators.iterate(_data);
    }
    
    int getLength(){
    	return _data.length;
    }
    
    @Override
    public String toString() {
        if(_clazz == null){
            return super.toString();    
        }
        return _clazz.toString(this);
    }
}
