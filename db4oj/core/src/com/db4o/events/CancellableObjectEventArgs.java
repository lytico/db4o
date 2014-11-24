/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.events;

import com.db4o.ext.*;
import com.db4o.internal.*;

/**
 * Argument for object related events which can be cancelled.
 * 
 * @see EventRegistry
 * @see CancellableEventArgs
 */
public class CancellableObjectEventArgs extends ObjectInfoEventArgs implements CancellableEventArgs {
	private boolean _cancelled;
	private Object _object;

	/**
	 * Creates a new instance for the specified object.
	 */
	public CancellableObjectEventArgs(Transaction transaction, ObjectInfo objectInfo, Object obj) {
		super(transaction, objectInfo);
		_object = obj;
	}
	
	/**
	 * @see CancellableEventArgs#cancel()
	 */
	public void cancel() {
		_cancelled = true;
	}

	/**
	 * @see CancellableEventArgs#isCancelled()
	 */
	public boolean isCancelled() {
		return _cancelled;
	}

	@Override
    public Object object() {
		return _object;
    }
	
	@Override
	public ObjectInfo info() {
		final ObjectInfo info = super.info();
		if (null == info) {
			throw new IllegalStateException();
		}
		
		return info;
	}
}
