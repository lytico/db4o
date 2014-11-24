/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.internal.*;


/** 
 * @exclude
 */
public class QConUnconditional extends QCon {

	// cannot be final for C/S unmarshalling
	private boolean _value;
	
    public QConUnconditional() {
        // C/S only
    }

    public QConUnconditional(Transaction trans, boolean value) {
        super(trans);
        _value = value;
    }
    
    void evaluateSimpleExec(QCandidates a_candidates) {
    	a_candidates.filter(this);
    }

	boolean evaluate(InternalCandidate a_candidate) {
		return _value;
	}

	@Override
	protected boolean canResolveByFieldIndex() {
		return false;
	}

}