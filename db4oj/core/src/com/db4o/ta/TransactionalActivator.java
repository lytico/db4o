package com.db4o.ta;

import com.db4o.activation.*;
import com.db4o.internal.*;

/**
 * An {@link Activator} implementation that activates an object on a specific
 * transaction.
 * 
 * @exclude
 */
final class TransactionalActivator implements Activator {
	
	private final Transaction _transaction;
	private final ObjectReference _objectReference;
	
	public TransactionalActivator(Transaction transaction, ObjectReference objectReference) {
		_objectReference = objectReference;
		_transaction = transaction;
	}

	public void activate(ActivationPurpose purpose) {
		_objectReference.activateOn(_transaction, purpose);
	}
}