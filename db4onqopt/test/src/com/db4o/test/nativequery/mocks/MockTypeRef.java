/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.test.nativequery.mocks;

import com.db4o.foundation.*;
import com.db4o.instrumentation.api.*;

public class MockTypeRef implements TypeRef {

	private final Class _type;

	public MockTypeRef(Class type) {
		_type = type;
	}

	public TypeRef elementType() {
		throw new NotImplementedException();
	}

	public boolean isPrimitive() {
		return _type.isPrimitive();
	}

	public String name() {
		return _type.getName();
	}
	
	public String toString() {
		return name();
	}
	
	public boolean equals(Object obj) {
		if (!(obj instanceof TypeRef)) {
			return false;
		}
		
		TypeRef other = (TypeRef)obj;
		return isPrimitive() == other.isPrimitive()
			&& name().equals(other.name());
	}
	
	public int hashCode() {
		return _type.hashCode();
	}
}
