/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.references;

import com.db4o.foundation.*;
import com.db4o.internal.*;



/**
 * @exclude
 */
public class TransactionalReferenceSystem extends TransactionalReferenceSystemBase implements ReferenceSystem {

	@Override
	public void commit() {
		traverseNewReferences(new Visitor4() {
			public void visit(Object obj) {
				ObjectReference oref = (ObjectReference)obj;
				if(oref.getObject() != null){
					_committedReferences.addExistingReference(oref);
				}
			}
		});
		createNewReferences();
	}

	@Override
	public void addExistingReference(ObjectReference ref) {
		_committedReferences.addExistingReference(ref);
	}

	@Override
	public void addNewReference(ObjectReference ref) {
		_newReferences.addNewReference(ref);
	}

	@Override
	public void removeReference(ObjectReference ref) {
		_newReferences.removeReference(ref);
		_committedReferences.removeReference(ref);
	}

	@Override
	public void rollback() {
		createNewReferences();
	}

	public void discarded() {
		// do nothing;
	}
	
}
