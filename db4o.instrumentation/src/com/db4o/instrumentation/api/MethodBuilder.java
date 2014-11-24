/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;

import java.io.*;
import java.lang.reflect.*;

/**
 * Cross platform interface for bytecode emission.
 */
public interface MethodBuilder {
	
	/**
	 * @sharpen.property
	 */
	ReferenceProvider references();
	
	void ldc(Object value);
	
	void loadArgument(int index);
	
	void pop();

	void loadArrayElement(TypeRef elementType);

	void add(TypeRef operandType);

	void subtract(TypeRef operandType);

	void multiply(TypeRef operandType);

	void divide(TypeRef operandType);

	void modulo(TypeRef operandType);

	void invoke(MethodRef method, CallingConvention convention);
	
	void invoke(Method method);	

	void loadField(FieldRef fieldRef);

	void loadStaticField(FieldRef fieldRef);
	
	void box(TypeRef boxedType);
	
	void endMethod();
	
	void print(PrintStream out);
}
