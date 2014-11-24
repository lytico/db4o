/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.references;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public abstract class TransactionalReferenceSystemBase {
	
	protected final ReferenceSystem _committedReferences;
	
	protected ReferenceSystem _newReferences;
	
	public TransactionalReferenceSystemBase() {
		createNewReferences();
		_committedReferences = newReferenceSystem();
	}
	
	private ReferenceSystem newReferenceSystem(){
	    return new HashcodeReferenceSystem();
	}

	public abstract void addExistingReference(ObjectReference ref);

	public abstract void addNewReference(ObjectReference ref);
	
	public abstract void commit();

	protected void traverseNewReferences(final Visitor4 visitor) {
		_newReferences.traverseReferences(visitor);
	}
	
	protected void createNewReferences(){
		_newReferences = newReferenceSystem();
	}

	public ObjectReference referenceForId(int id) {
		ObjectReference ref = _newReferences.referenceForId(id);
		if(ref != null){
			return ref;
		}
		return _committedReferences.referenceForId(id);
	}

	public ObjectReference referenceForObject(Object obj) {
		ObjectReference ref = _newReferences.referenceForObject(obj);
		if(ref != null){
			return ref;
		}
		return _committedReferences.referenceForObject(obj);
	}

	public abstract void removeReference(ObjectReference ref);
	
	public abstract void rollback();
	
	public void traverseReferences(Visitor4 visitor) {
		traverseNewReferences(visitor);
		_committedReferences.traverseReferences(visitor);
	}



}
