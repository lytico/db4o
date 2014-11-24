/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree.algebra;

import com.db4o.internal.btree.*;

/**
 * @exclude
 */
public class BTreeRangeUnionIntersect extends BTreeRangeUnionOperation {

	public BTreeRangeUnionIntersect(BTreeRangeUnion union) {
		super(union);
	}

	protected BTreeRange execute(BTreeRangeSingle range) {
		return BTreeAlgebra.intersect(_union, range);
	}

	protected BTreeRange execute(BTreeRangeUnion union) {
		return BTreeAlgebra.intersect(_union, union);
	}

}
