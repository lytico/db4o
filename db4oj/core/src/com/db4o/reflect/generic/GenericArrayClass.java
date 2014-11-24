/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;


/**
 * @exclude
 */
public class GenericArrayClass extends GenericClass {
    
    public GenericArrayClass(GenericReflector reflector, ReflectClass delegateClass, String name, GenericClass superclass) {
        super(reflector, delegateClass, name, superclass);
    }
    
    public ReflectClass getComponentType() {
        return getDelegate();
    }
    
    public boolean isArray() {
        return true;
    }
    
    public boolean isInstance(Object candidate) {
        if (!(candidate instanceof GenericArray)) {
            return false;
        }
        return isAssignableFrom(((GenericArray)candidate)._clazz);
    }
    
    public String toString(Object obj) {
    	if(_converter == null) {
    		return "(GA) " + getName();
    	}
    	return _converter.toString((GenericArray) obj);
    }
    
}
