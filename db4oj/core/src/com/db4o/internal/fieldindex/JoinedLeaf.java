/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.query.processor.*;

public abstract class JoinedLeaf implements IndexedNodeWithRange {

	private final QCon _constraint;
	private final IndexedNodeWithRange _leaf1;
	private final BTreeRange _range;
	
	public JoinedLeaf(final QCon constraint, final IndexedNodeWithRange leaf1, final BTreeRange range) {
		if (null == constraint || null == leaf1 || null == range) {
			throw new ArgumentNullException();
		}
		_constraint = constraint;
		_leaf1 = leaf1;
		_range = range;
	}
	
	public QCon getConstraint() {
		return _constraint;
	}
	
	public BTreeRange getRange() {
		return _range;
	}

	public Iterator4 iterator() {
		return _range.keys();
	}

	public BTree getIndex() {
		return _leaf1.getIndex();
	}

	public boolean isResolved() {
		return _leaf1.isResolved();
	}

	public IndexedNode resolve() {
		return IndexedPath.newParentPath(this, _constraint);
	}

	public int resultSize() {
		return _range.size();
	}
	
	public boolean isEmpty(){
		return _range.isEmpty();
	}
	
	public void markAsBestIndex(QCandidates candidates) {
		_leaf1.markAsBestIndex(candidates);
		_constraint.setProcessedByIndex(candidates);
	}
	
	public void traverse(IntVisitor visitor){
		IndexedNodeBase.traverse(this, visitor);
	}

}