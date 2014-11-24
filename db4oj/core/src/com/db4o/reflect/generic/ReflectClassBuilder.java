/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;

/**
 * @exclude
 */
public interface ReflectClassBuilder {
	ReflectClass createClass(String name,ReflectClass superClass,int fieldCount);
	ReflectField createField(ReflectClass parentType,String fieldName,ReflectClass fieldType,boolean isVirtual,boolean isPrimitive,boolean isArray, boolean isNArray);
	void initFields(ReflectClass clazz,ReflectField[] fields);
	ReflectField[] fieldArray(int length);
}
