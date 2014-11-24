/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.events;



import com.db4o.cs.internal.*;
import com.db4o.events.*;
import com.db4o.internal.events.*;

/**
 * @sharpen.partial
 */
public class ClientEventRegistryImpl extends EventRegistryImpl {
	
	private final ClientObjectContainer _container;

	public ClientEventRegistryImpl(ClientObjectContainer container) {
		_container = container;
	}
	
	@Override
	protected void onCommittedListenerAdded() {
		_container.onCommittedListenerAdded();
	}

	/**
	 * @sharpen.ignore
	 */
	public Event4 deleted() {
		throw new IllegalArgumentException("delete() events are raised only at server side.");
	}
	
	/**
	 * @sharpen.ignore
	 */
	public Event4 deleting() {
		throw new IllegalArgumentException("deleting() events are raised only at server side.");
	}
}
