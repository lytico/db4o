/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.references;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public interface ReferenceSystem {

	public void addNewReference(ObjectReference ref);

	public void addExistingReference(ObjectReference ref);

	public void commit();
	
	public ObjectReference referenceForId(int id);

	public ObjectReference referenceForObject(Object obj);
	
	public void removeReference(ObjectReference ref);
	
	public void rollback();
	
	public void traverseReferences(Visitor4 visitor);

	public void discarded();

}