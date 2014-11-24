/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import java.lang.reflect.*;

import com.db4o.foundation.*;
import com.db4o.instrumentation.api.*;

public class BloatReferenceResolver implements ReferenceResolver {
	
	private final NativeClassFactory _loader;

	public BloatReferenceResolver(NativeClassFactory loader) {
		if (null == loader) throw new ArgumentNullException();
		_loader = loader;
	}

	public Method resolve(MethodRef methodRef) {
		final Class declaringClass = resolve(methodRef.declaringType());
		final Class[] paramTypes = resolve(methodRef.paramTypes());
		try {
			return declaringClass.getDeclaredMethod(methodRef.name(), paramTypes);
		} catch (Exception e) {
			throw new InstrumentationException(e);
		}
	}

	private Class[] resolve(TypeRef[] paramTypes) {
		Class[] classes = new Class[paramTypes.length];
		for (int i=0; i<paramTypes.length; ++i) {
			classes[i] = resolve(paramTypes[i]);
		}
		return classes;
	}

	private Class resolve(TypeRef typeRef) {
		try {
			return _loader.forName(typeRef.name());
		} catch (ClassNotFoundException e) {
			throw new InstrumentationException(e);
		}
	}

}
