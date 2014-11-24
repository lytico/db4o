/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class TreeKeyIterator extends AbstractTreeIterator {
	public TreeKeyIterator(Tree tree) {
		super(tree);
	}

	protected Object currentValue(Tree tree) {
		return tree.key();
	}
}
