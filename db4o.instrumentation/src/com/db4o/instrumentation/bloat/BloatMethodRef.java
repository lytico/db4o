/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.api.*;

public class BloatMethodRef extends BloatMemberRef implements MethodRef {

	private TypeRef[] _paramTypes;

	BloatMethodRef(BloatReferenceProvider provider, MemberRef method) {
		super(provider, method);
	}
	
	public TypeRef declaringType() {
		return typeRef(_member.declaringClass());
	}

	public TypeRef returnType() {
		return typeRef(_member.type().returnType());
	}

	public TypeRef[] paramTypes() {
		if (null == _paramTypes) {
			_paramTypes = buildParamTypes();
		}
		return _paramTypes;
	}

	private TypeRef[] buildParamTypes() {
		Type[] paramTypes = _member.type().paramTypes();
		TypeRef[] types = new TypeRef[paramTypes.length];
		for (int i=0; i<paramTypes.length; ++i) {
			types[i] = typeRef(paramTypes[i]);
		}
		return types;
	}
}
