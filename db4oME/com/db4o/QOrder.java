/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
class QOrder extends Tree{
	
	final QConObject _constraint;
	final QCandidate _candidate;
	
	QOrder(QConObject a_constraint, QCandidate a_candidate){
		_constraint = a_constraint;
		_candidate = a_candidate;
	}

	public int compare(Tree a_to) {
		if(_constraint.i_comparator.isSmaller(_candidate.value())){
			return _constraint.i_orderID;	
		}
		if(_constraint.i_comparator.isEqual(_candidate.value())){
			return 0;	
		}
		return - _constraint.i_orderID;	
	}

	public Object shallowClone() {
		QOrder order= new QOrder(_constraint,_candidate);
		super.shallowCloneInternal(order);
		return order;
	}
}

