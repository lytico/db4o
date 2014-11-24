/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;

/**
 * @exclude
 */
public class GenericVirtualField extends GenericField{

    public GenericVirtualField(String name) {
        super(name, null, false, false, false);
    }
    
    public Object deepClone(Object obj) {
        Reflector reflector = (Reflector)obj;
        return new GenericVirtualField(getName());
    }

    public Object get(Object onObject) {
        return null;
    }

    public ReflectClass getType() {
        return null;
    }

    public boolean isPublic() {
        return false;
    }

    public boolean isStatic() {
        return true;
    }

    public boolean isTransient() {
        return true;
    }

    public void set(Object onObject, Object value) {
        // do nothing
    }
}
