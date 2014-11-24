/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.events;

import com.db4o.internal.*;

/**
 * Arguments for object related events.
 * 
 * @see EventRegistry
 */
public abstract class ObjectEventArgs extends TransactionalEventArgs {

	/**
	 * Creates a new instance for the specified object.
	 */
	protected ObjectEventArgs(Transaction transaction) {
		super(transaction);
	}

	/**
	 * The object that triggered this event.
	 * 
	 * @sharpen.property
	 */
	public abstract Object object();
}
