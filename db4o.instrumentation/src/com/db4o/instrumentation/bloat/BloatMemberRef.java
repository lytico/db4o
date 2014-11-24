/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.bloat;

import EDU.purdue.cs.bloat.editor.*;

public class BloatMemberRef extends BloatRef {

	protected final MemberRef _member;
	
	public BloatMemberRef(BloatReferenceProvider provider, MemberRef memberRef) {
		super(provider);
		_member = memberRef;
	}
	
	public String name() {
		return _member.name();
	}

	public MemberRef member() {
		return _member;
	}
	
	public String toString() {
		return name();
	}

	public static MemberRef memberRef(Object memberRef) {
		return ((BloatMemberRef)memberRef).member();
	}

}