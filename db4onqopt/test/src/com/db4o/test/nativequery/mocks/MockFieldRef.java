/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.test.nativequery.mocks;

import com.db4o.foundation.*;
import com.db4o.instrumentation.api.*;

public class MockFieldRef implements FieldRef {

	private final String _name;
	private final TypeRef _type;

	public MockFieldRef(String name) {
		this(name, new MockTypeRef(Object.class));
	}

	public MockFieldRef(String name, TypeRef typeRef) {
		if (null == name) throw new ArgumentNullException();
		if (null == typeRef) throw new ArgumentNullException();
		_name = name;
		_type = typeRef;
	}

	public String name() {
		return _name;
	}

	public TypeRef type() {
		return _type;
	}
	
	public String toString() {
		return name();
	}
	
	public boolean equals(Object obj) {
		if (!(obj instanceof FieldRef)) {
			return false;
		}
		FieldRef other = (FieldRef)obj;
		return _name.equals(other.name())
			&& _type.equals(other.type());
	}
	
	public int hashCode() {
		return _name.hashCode() + 29*_type.hashCode();
	}
}
