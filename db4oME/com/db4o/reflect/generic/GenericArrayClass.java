/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;


/**
 * @exclude
 */
public class GenericArrayClass extends GenericClass {
    
    public GenericArrayClass(GenericReflector reflector, ReflectClass delegateClass, String name, GenericClass superclass) {
        super(reflector, delegateClass, "(GA) " + name, superclass);
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
        return isAssignableFrom(((GenericObject)candidate)._class);
    }
    
    public boolean equals(Object obj) {
        if( ! (obj instanceof GenericArrayClass)){
            return false;
        }
        return super.equals(obj);
    }
    
}
