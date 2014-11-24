/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.test.nativequery.mocks;

import java.lang.reflect.*;

import com.db4o.instrumentation.api.*;

import db4ounit.*;

public class MockMethodRef implements MethodRef {

	private final Method _method;

	public MockMethodRef(Method method) {
		_method = method;
	}

	public String name() {
		return _method.getName();
	}

	public TypeRef[] paramTypes() {
		final Class[] paramTypes = _method.getParameterTypes();
		final TypeRef[] types = new TypeRef[paramTypes.length];
		for (int i=0; i<paramTypes.length; ++i) {
			types[i] = typeRef(paramTypes[i]);
		}
		return types;
	}
	
	public TypeRef declaringType() {
		return typeRef(_method.getDeclaringClass());
	}

	private TypeRef typeRef(Class type) {
		return new MockTypeRef(type);
	}

	public TypeRef returnType() {
		return typeRef(_method.getReturnType());
	}
	
	public String toString() {
		return name();
	}
	
	public boolean equals(Object obj) {
		if (!(obj instanceof MethodRef)) {
			return false;
		}
		
		MethodRef other = (MethodRef)obj;
		return name().equals(other.name())
			&& Check.objectsAreEqual(declaringType(), other.declaringType())
			&& Check.objectsAreEqual(returnType(), other.returnType())
			&& Check.arraysAreEqual(paramTypes(), other.paramTypes());
	}	
}
