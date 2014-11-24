/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.internal.btree;

public class BTreeRangePointerIterator extends AbstractBTreeRangeIterator {
	
	public BTreeRangePointerIterator(BTreeRangeSingle range) {
		super(range);
	}

	public Object current() {
		return currentPointer();
	}
}
