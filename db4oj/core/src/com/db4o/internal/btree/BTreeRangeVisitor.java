/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.btree;

/**
 * @exclude
 */
public interface BTreeRangeVisitor {

	void visit(BTreeRangeSingle range);

	void visit(BTreeRangeUnion union);

}