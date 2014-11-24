/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.events;

import com.db4o.ext.*;
import com.db4o.internal.*;

/**
 * Arguments for commit time related events.
 * 
 * @see EventRegistry
 */
public class CommitEventArgs extends TransactionalEventArgs {
	
	private final CallbackObjectInfoCollections _collections;
	private final boolean _isOwnCommit;

	public CommitEventArgs(Transaction transaction, CallbackObjectInfoCollections collections, boolean isOwnCommit) {
		super(transaction);
		_collections = collections;
		_isOwnCommit = isOwnCommit;
	}
	
	/**
	 * Returns a iteration
	 * 
	 * @sharpen.property
	 */
	public ObjectInfoCollection added() {
		return _collections.added;
	}
	
	/**
	 * @sharpen.property
	 */
	public ObjectInfoCollection deleted() {
		return _collections.deleted;
	}

	/**
	 * @sharpen.property
	 */
	public ObjectInfoCollection updated() {
		return _collections.updated;
	}

	public boolean isOwnCommit() {
		return _isOwnCommit;
	}
}
