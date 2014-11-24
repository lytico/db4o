/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.query.processor.*;

public interface IndexedNode extends Iterable4, IntVisitable{

	boolean isResolved();

	IndexedNode resolve();

	BTree getIndex();
	
	int resultSize();

	void markAsBestIndex(QCandidates candidates);

	boolean isEmpty();
	
}