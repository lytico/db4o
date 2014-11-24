/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.internal.query.processor;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.query.*;

public interface InternalCandidate extends Candidate {
	InternalCandidate getRoot();
	ClassMetadata classMetadata();
	QCandidates candidates();
	LocalTransaction transaction();
	boolean evaluate(QConObject qConObject, QE evaluator);
	boolean fieldIsAvailable();
	int id();
	PreparedComparison prepareComparison(ObjectContainerBase container, Object constraint);
	boolean evaluate(QPending pending);
	void doNotInclude();
	void root(InternalCandidate root);
	boolean include();
	Tree pendingJoins();
}
