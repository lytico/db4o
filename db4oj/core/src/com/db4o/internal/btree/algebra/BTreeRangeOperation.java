/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree.algebra;

import com.db4o.internal.btree.*;

/**
 * @exclude
 */
public abstract class BTreeRangeOperation implements BTreeRangeVisitor {

	private BTreeRange _resultingRange;

	public BTreeRangeOperation() {
		super();
	}

	public BTreeRange dispatch(BTreeRange range) {
		range.accept(this);
		return _resultingRange;
	}
	
	public final void visit(BTreeRangeSingle single) {
		_resultingRange = execute(single);
	}
	
	public final void visit(BTreeRangeUnion union) {
		_resultingRange = execute(union);
	}

	protected abstract BTreeRange execute(BTreeRangeUnion union);

	protected abstract BTreeRange execute(BTreeRangeSingle single);

}