/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.internal.query.processor;


public interface ParentCandidate extends InternalCandidate {
	boolean createChild(QField field, final QCandidates candidates);
	void useField(QField field);
}
