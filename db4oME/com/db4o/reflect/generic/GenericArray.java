/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

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
    
    int getLength(){
        return _data.length;
    }
}
