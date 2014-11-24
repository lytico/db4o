/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.internal.query.processor.*;

public class AndIndexedLeaf extends JoinedLeaf {

	public AndIndexedLeaf(QCon constraint, IndexedNodeWithRange leaf1, IndexedNodeWithRange leaf2) {
		super(constraint, leaf1, leaf1.getRange().intersect(leaf2.getRange()));
	}
}
