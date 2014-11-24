/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import java.lang.reflect.*;
import java.util.*;

import EDU.purdue.cs.bloat.editor.*;
import EDU.purdue.cs.bloat.editor.Type;

import com.db4o.instrumentation.api.*;

public class BloatReferenceProvider implements ReferenceProvider {
	
	private Hashtable _types = new Hashtable();

	public MethodRef forMethod(TypeRef declaringType, String methodName, TypeRef[] parameterTypes, TypeRef returnType) {
		Type[] argTypes=BloatTypeRef.bloatTypes(parameterTypes);
		NameAndType nameAndType=new NameAndType(methodName, Type.getType(argTypes, bloatType(returnType)));
		return forBloatMethod(new MemberRef(bloatType(declaringType), nameAndType));
	}

	Type bloatType(Class clazz) {
		return Type.getType(clazz);
	}
	
	Type bloatType(TypeRef type) {
		return BloatTypeRef.bloatType(type);
	}

	public TypeRef forType(Class type) {
		return forBloatType(bloatType(type));
	}

	public MethodRef forMethod(Method method) {
		return forMethod(forType(method.getDeclaringClass()), method.getName(), forTypes(method.getParameterTypes()), forType(method.getReturnType()));
	}

	private TypeRef[] forTypes(Class[] types) {
		TypeRef[] typeRefs = new TypeRef[types.length];
		for (int i=0; i<types.length; ++i) {
			typeRefs[i] = forType(types[i]);
		}
		return typeRefs;
	}

	public MethodRef forBloatMethod(MemberRef method) {
		return new BloatMethodRef(this, method);
	}

	public TypeRef forBloatType(Type type) {
		BloatTypeRef typeRef = (BloatTypeRef)_types.get(type.descriptor());
		if (null == typeRef) {
			typeRef = new BloatTypeRef(this, type);
			_types.put(type.descriptor(), typeRef);
		}
		return typeRef;
	}

	public FieldRef forBloatField(MemberRef field) {
		return new BloatFieldRef(this, field);
	}

}
