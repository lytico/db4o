/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.internal.*;

/**
 * @exclude
 */
public interface BTreeStructureListener {

	void notifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode);

	void notifyDeleted(Transaction trans, BTreeNode node);

	void notifyCountChanged(Transaction trans, BTreeNode node, int diff);

}
