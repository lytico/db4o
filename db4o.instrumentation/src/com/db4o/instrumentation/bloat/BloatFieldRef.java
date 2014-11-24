/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import EDU.purdue.cs.bloat.editor.*;

import com.db4o.instrumentation.api.*;

public class BloatFieldRef extends BloatMemberRef implements FieldRef {

	BloatFieldRef(BloatReferenceProvider provider, MemberRef fieldRef) {
		super(provider, fieldRef);
	}

	public TypeRef type() {
		return typeRef(_member.type());
	}
}
