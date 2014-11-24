/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree.algebra;

import com.db4o.internal.btree.*;

/**
 * @exclude
 */
public abstract class BTreeRangeSingleOperation extends BTreeRangeOperation {

	protected final BTreeRangeSingle _single;

	public BTreeRangeSingleOperation(BTreeRangeSingle single) {
		_single = single;
	}

}