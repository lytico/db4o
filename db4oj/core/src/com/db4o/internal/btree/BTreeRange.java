/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.foundation.*;


public interface BTreeRange {
	
	/**
	 * Iterates through all the valid pointers in 
	 * this range.
	 * @return an Iterator4 over BTreePointer value
	 */
	public Iterator4 pointers();
	
	public Iterator4 keys();

	public int size();

	public BTreeRange greater();

	public BTreeRange union(BTreeRange other);

	public BTreeRange extendToLast();

	public BTreeRange smaller();

	public BTreeRange extendToFirst();

	public BTreeRange intersect(BTreeRange range);

	public BTreeRange extendToLastOf(BTreeRange upperRange);

	public boolean isEmpty();
	
	public void accept(BTreeRangeVisitor visitor);

	public BTreePointer lastPointer();
}
