/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree.algebra;

import com.db4o.internal.btree.*;

/**
 * @exclude
 */
public class BTreeRangeUnionUnion extends BTreeRangeUnionOperation {
	
	public BTreeRangeUnionUnion(BTreeRangeUnion union) {
		super(union);
	}

	protected BTreeRange execute(BTreeRangeUnion union) {
		return BTreeAlgebra.union(_union, union);
	}

	protected BTreeRange execute(BTreeRangeSingle single) {
		return BTreeAlgebra.union(_union, single);
	}
}
