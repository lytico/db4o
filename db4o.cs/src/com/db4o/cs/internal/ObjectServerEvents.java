package com.db4o.cs.internal;

import com.db4o.events.*;

public interface ObjectServerEvents {

	/**
	 * @sharpen.event ClientConnectionEventArgs
	 */
	public Event4<ClientConnectionEventArgs> clientConnected();
	
	/**
	 * @return an event that provides the name of the client being disconnected.
	 *  
     * @since 7.12
     *
	 * @sharpen.event Db4objects.Db4o.Events.StringEventArgs
	 */
	public Event4<StringEventArgs> clientDisconnected();
	
	/**
	 * @sharpen.event ServerClosedEventArgs
	 */
	public Event4<ServerClosedEventArgs> closed();

}
