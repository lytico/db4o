/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

import java.lang.reflect.*;


public interface ReferenceProvider {
	
	TypeRef forType(Class type);
	
	MethodRef forMethod(Method method);
	
	MethodRef forMethod(TypeRef declaringType, String methodName, TypeRef[] parameterTypes, TypeRef returnType);
}
