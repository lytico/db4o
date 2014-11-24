/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree.algebra;

import com.db4o.internal.btree.*;


/**
 * @exclude
 */
public class BTreeRangeSingleUnion extends BTreeRangeSingleOperation{

	public BTreeRangeSingleUnion(BTreeRangeSingle single) {
		super(single);
	}

	protected BTreeRange execute(BTreeRangeSingle single) {
		return BTreeAlgebra.union(_single, single);
	}

	protected BTreeRange execute(BTreeRangeUnion union) {		 
		return BTreeAlgebra.union(union, _single);
	}
}
