/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree.algebra;

import com.db4o.internal.btree.*;

/**
 * @exclude
 */
public abstract class BTreeRangeUnionOperation extends BTreeRangeOperation {

	protected final BTreeRangeUnion _union;

	public BTreeRangeUnionOperation(BTreeRangeUnion union) {
		_union = union;
	}

}