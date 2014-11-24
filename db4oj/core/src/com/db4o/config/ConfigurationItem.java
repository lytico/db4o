/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

import com.db4o.internal.*;

/**
 * Implement this interface for configuration items that encapsulate
 * a batch of configuration settings or that need to be applied 
 * to ObjectContainers after they are opened. 
 */
public interface ConfigurationItem {
	
	/**
	 * Gives a chance for the item to augment the configuration.
	 * 
	 * @param configuration the configuration that the item was added to
	 */
	public void prepare(Configuration configuration);
	
	/**
	 * Gives a chance for the item to configure the just opened ObjectContainer.
	 * 
	 * @param container the ObjectContainer to configure
	 */
	public void apply(InternalObjectContainer container);
	
}
