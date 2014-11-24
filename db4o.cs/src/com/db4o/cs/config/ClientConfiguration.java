/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.cs.config;

import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.messaging.*;

/**
 * Configuration interface for db4o networking clients.
 * 
 * @since 7.5
 */
public interface ClientConfiguration extends NetworkingConfigurationProvider, CommonConfigurationProvider {
	
	/**
	 * Sets the number of IDs to be pre-allocated in the database for new 
	 * objects created on the client.
	 * 
	 * @param prefetchIDCount
	 *            The number of IDs to be prefetched
	 *            
	 * @sharpen.property
	 */
	void prefetchIDCount(int prefetchIDCount);

	/**
	 * Sets the number of objects to be prefetched for an ObjectSet.
	 * 
	 * @param prefetchObjectCount
	 *            The number of objects to be prefetched
	 *            
	 * @sharpen.property
	 */
	void prefetchObjectCount(int prefetchObjectCount);
	
	/**
	 * returns the MessageSender for this Configuration context.
	 * This setting should be used on the client side.
	 * @return MessageSender
	 * 
	 * @sharpen.property
	 */
	public MessageSender messageSender();

	/**
	 * Sets the depth to which prefetched objects will be activated.
	 *
	 * 
	 * @sharpen.property
	 */
	void prefetchDepth(int prefetchDepth);

	/**
	 * Sets the slot cache size to the given value.
	 * 
	 * @param slotCacheSize
	 * 
	 * @sharpen.property
	 */
	void prefetchSlotCacheSize(int slotCacheSize);

	/**
	 * configures the time a client waits for a message response 
	 * from the server. <br>
	 * <br>
	 * Default value: 600000ms (10 minutes)<br>
	 * <br>
     * It is recommended to use the same values for {@link #timeoutClientSocket(int)}
     * and {@link ServerConfiguration#timeoutServerSocket(int)}.
     * <br>
	 * @param milliseconds
	 *            time in milliseconds
	 *            
	 * @sharpen.property
	 */
	public void timeoutClientSocket(int milliseconds);

	/**
     * adds ConfigurationItems to be applied when
     * a networking {@link ClientObjectContainer} is opened. 
     * @param configItem the {@link ClientConfigurationItem}
     * @since 7.12
     */
	public void addConfigurationItem(ClientConfigurationItem configItem);
}
