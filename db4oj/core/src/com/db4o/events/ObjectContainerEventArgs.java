/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.events;

import com.db4o.*;

/**
 * Arguments for container related events.
 * 
 * @see EventRegistry
 */
public class ObjectContainerEventArgs extends EventArgs {

	private final ObjectContainer _container;

	public ObjectContainerEventArgs(ObjectContainer container) {
		_container = container;
	}
	
	/**
	 * @sharpen.property
	 */
	public ObjectContainer objectContainer() {
		return _container;
	}
}
