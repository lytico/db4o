/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.classindex;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public interface ClassIndexStrategy {
	
	void initialize(ObjectContainerBase stream);
	
	void read(ObjectContainerBase stream, int indexID);
	
	int write(Transaction transaction);
	
	void add(Transaction transaction, int id);
	
	void remove(Transaction transaction, int id);
	
	int entryCount(Transaction transaction);
	
	int ownLength();
	
	void purge();
	
	/**
	 * Traverses all index entries (java.lang.Integer references).
	 */
	void traverseIds(Transaction transaction,Visitor4 command);
	
	void dontDelete(Transaction transaction, int id);
	
	Iterator4 allSlotIDs(Transaction trans);
	// FIXME: Why is this never called?
	void defragReference(ClassMetadata classMetadata,DefragmentContextImpl context,int classIndexID);
	
	int id();
	// FIXME: Why is this never called?
	void defragIndex(DefragmentContextImpl context);
	
	IntVisitable idVisitable(Transaction trans);
}

