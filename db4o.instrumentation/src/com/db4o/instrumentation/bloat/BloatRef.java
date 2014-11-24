/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.api.*;

public class BloatRef {

	private final BloatReferenceProvider _provider;

	public BloatRef(BloatReferenceProvider provider) {
		_provider = provider;
	}

	protected TypeRef typeRef(Type type) {
		return _provider.forBloatType(type);
	}
	
}