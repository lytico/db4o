/* Copyright (C) 2005   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.foundation.*;
import com.db4o.reflect.*;

/**
 * @exclude
 */
public class GenericField implements ReflectField, DeepClone{

    private final String _name;
    private final GenericClass _type;
    private final boolean _primitive;

    private int _index = -1;

    public GenericField(String name, ReflectClass clazz, boolean primitive) {
        _name = name;
        _type = (GenericClass)clazz;
        _primitive = primitive;
    }

    public Object deepClone(Object obj) {
        Reflector reflector = (Reflector)obj;
        ReflectClass newReflectClass = null;
        if(_type != null){
            newReflectClass = reflector.forName(_type.getName());
        }
        return new GenericField(_name, newReflectClass, _primitive);
    }

    public Object get(Object onObject) {
        //TODO Consider: Do we need to check that onObject is an instance of the DataClass this field is a member of? 
        return ((GenericObject)onObject).get(_index);
    }
    
    public String getName() {
        return _name;
    }

    public ReflectClass getFieldType() {
        return _type;
    }

    public boolean isPublic() {
        return true;
    }
    
    public boolean isPrimitive(){
        return _primitive;
    }

    public boolean isStatic() { //FIXME Consider static fields.
        return false;
    }

    public boolean isTransient() {
        return false;
    }

    public void set(Object onObject, Object value) {
		// FIXME: Consider enabling type checking.
		// The following will fail with arrays.
        // if (!_type.isInstance(value)) throw new RuntimeException(); //TODO Consider: is this checking really necessary?
        ((GenericObject)onObject).set(_index,value);
    }

    void setIndex(int index) {
        _index = index;
    }

	public Object indexEntry(Object orig) {
		return orig;
	}

	public ReflectClass indexType() {
		return getFieldType();
	}
}
