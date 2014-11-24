/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.api.*;


public class BloatTypeEditor implements TypeEditor {

	private final ClassEditor _classEditor;
	private final BloatReferenceProvider _references;

	public BloatTypeEditor(ClassEditor classEditor, BloatReferenceProvider references) {
		_classEditor = classEditor;
		_references = references;
	}

	public TypeRef type() throws InstrumentationException {
		return _references.forBloatType(_classEditor.type());
	}

	public void addInterface(TypeRef type) {
		_classEditor.addInterface(BloatTypeRef.bloatType(type));
	}

	public MethodBuilder newPublicMethod(String methodName, TypeRef returnType, TypeRef[] parameterTypes) {
		return new BloatMethodBuilder(_references, _classEditor, methodName, returnType, parameterTypes);
	}

	public ReferenceProvider references() {
		return _references;
	}
}
