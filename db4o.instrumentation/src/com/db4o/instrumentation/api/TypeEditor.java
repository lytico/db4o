/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

/**
 * Cross platform interface for type instrumentation.
 */
public interface TypeEditor {
	
	/**
	 * @sharpen.property
	 */
	TypeRef type();
	
	/**
	 * @sharpen.property
	 */
	ReferenceProvider references();
	
	void addInterface(TypeRef type);
	
	MethodBuilder newPublicMethod(String methodName, TypeRef returnType, TypeRef[] parameterTypes);
}
