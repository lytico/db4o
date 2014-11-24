/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;

/**
 * @exclude
 */
public class GenericClassBuilder implements ReflectClassBuilder {

	private GenericReflector _reflector;
	private Reflector _delegate;
	
	public GenericClassBuilder(GenericReflector reflector, Reflector delegate_) {
		super();
		_reflector = reflector;
		_delegate = delegate_;
	}

	public ReflectClass createClass(String name, ReflectClass superClass, int fieldCount) {
		ReflectClass nativeClass = _delegate.forName(name);
		GenericClass clazz=new GenericClass(_reflector, nativeClass,name, (GenericClass)superClass);
		clazz.setDeclaredFieldCount(fieldCount);
		return clazz;
	}

	public ReflectField createField(
			ReflectClass parentType, 
			String fieldName,
			ReflectClass fieldType, 
			boolean isVirtual, 
			boolean isPrimitive,
			boolean isArray, boolean isNArray) {
        if (isVirtual) {
            return new GenericVirtualField(fieldName);
        }   
		return new GenericField(fieldName, fieldType, isPrimitive);
	}

	public void initFields(ReflectClass clazz, ReflectField[] fields) {
        ((GenericClass)clazz).initFields((GenericField[])fields);
	}

	public ReflectField[] fieldArray(int length) {
		return new GenericField[length];
	}
}
