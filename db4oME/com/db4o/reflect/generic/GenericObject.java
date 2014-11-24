/* Copyright (C) 2005   db4objects Inc.   http://www.db4o.com */

package com.db4o.reflect.generic;


/**
 * @exclude
 */
public class GenericObject {

    final GenericClass _class;
    
    private Object[] _values;
    
    public GenericObject(GenericClass clazz) {
        _class = clazz;
    }
    
    private void ensureValuesInitialized() {
    	if(_values == null) {
    		_values = new Object[_class.getFieldCount()];
    	}
    }
    
    public void set(int index,Object value) {
    	ensureValuesInitialized();
    	_values[index]=value;
    }

    public Object get(int index) {
    	ensureValuesInitialized();
    	return _values[index];
    }

    public String toString(){
        if(_class == null){
            return super.toString();    
        }
        return _class.toString(this);
    }
}
