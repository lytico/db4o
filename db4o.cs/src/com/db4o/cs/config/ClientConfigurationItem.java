/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.cs.config;

import com.db4o.ext.*;

/**
 * Implement this interface for configuration items that encapsulate
 * a batch of configuration settings or that need to be applied 
 * to ClientObjectContainers after they are opened.
 * 
 * @since 7.12
 */
public interface ClientConfigurationItem {
	/**
	 * Gives a chance for the item to augment the configuration.
	 * 
	 * @param configuration the configuration that the item was added to
	 */
	public void prepare(ClientConfiguration configuration);
	
	/**
	 * Gives a chance for the item to configure the just opened ObjectContainer.
	 * 
	 * @param client the ObjectContainer to configure
	 */
	public void apply(ExtClient client);

}
