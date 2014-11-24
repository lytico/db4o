package com.db4o.ta.instrumentation;

import EDU.purdue.cs.bloat.editor.MemberRef;

import com.db4o.activation.ActivationPurpose;

public final class FieldAccess {

	public final MemberRef fieldRef;
	
	public final ActivationPurpose purpose;

	public FieldAccess(MemberRef fieldRef_, ActivationPurpose purpose_) {
		fieldRef = fieldRef_;
		purpose = purpose_;
	}

}

