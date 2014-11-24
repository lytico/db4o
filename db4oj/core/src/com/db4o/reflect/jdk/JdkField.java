/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect.jdk;

import java.lang.reflect.*;

import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

/**
 * Reflection implementation for Field to map to JDK reflection.
 * 
 * @sharpen.ignore
 */
public class JdkField implements ReflectField {

    private final Reflector reflector;
	private final Field field;

    public JdkField(Reflector reflector_, Field field_) {
    	reflector = reflector_;
        field = field_;
        setAccessible();
    }

    public String getName() {
        return field.getName();
    }

    public ReflectClass getFieldType() {
        return reflector.forClass(field.getType());
    }

    public boolean isPublic() {
        return Modifier.isPublic(field.getModifiers());
    }

    public boolean isStatic() {
        return Modifier.isStatic(field.getModifiers());
    }

    public boolean isTransient() {
        return Modifier.isTransient(field.getModifiers());
    }

    private void setAccessible() {
        Platform4.setAccessible(field);
    }

    public Object get(Object onObject) {
    	try {
			return field.get(onObject);
		} 
    	catch (Exception exc) {
			return handleException(exc);
		}
    }

    public void set(Object onObject, Object attribute) {
    	try {
			field.set(onObject, attribute);
		} catch (Exception exc) {
			handleException(exc);
		}
    }

	public Object indexEntry(Object orig) {
		return orig;
	}

	public ReflectClass indexType() {
		return getFieldType();
	}
	
	public String toString() {
	    return "JDKField " + getFieldType().getName() + ":" + getName();
	}
	
	private Object handleException(Exception exc) {
		if(!isSynthetic()) {
			throw new Db4oException(toString(), exc);
		}
		return null;
	}
	
	private boolean isSynthetic() {
		return field.getName().startsWith("class$")
			|| field.getName().startsWith("this$"); // confirmed on MacOSX jdk 1.4
	}
}
