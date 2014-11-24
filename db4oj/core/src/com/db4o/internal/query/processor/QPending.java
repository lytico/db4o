/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.query.processor;

import com.db4o.foundation.*;


/**
 * @exclude
 */
public class QPending extends Tree{
	
	final QConJoin			_join;
	QCon 					_constraint;
	
	int 					_result;

	// Constants, so QConJoin.evaluatePending is made easy:
	static final int FALSE = -4;
	static final int BOTH = 1;
	static final int TRUE = 2;
	
	QPending(QConJoin a_join, QCon a_constraint, boolean a_firstResult){
		_join = a_join;
		_constraint = a_constraint;
		
		_result = a_firstResult ? TRUE : FALSE;
	}
	
	public int compare(Tree a_to) {
		return _constraint.id() - ((QPending)a_to)._constraint.id();
	}

	void changeConstraint(){
		_constraint = _join.getOtherConstraint(_constraint);
	}

	public Object shallowClone() {
		QPending pending = internalClonePayload();
		super.shallowCloneInternal(pending);
		return pending;
	}

	QPending internalClonePayload() {
		QPending pending = new QPending(_join, _constraint, false);
		pending._result=_result;
		return pending;
	}
	
    public Object key(){
    	throw new NotImplementedException();
    }

}

