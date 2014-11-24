package com.db4o.cs.internal;

import com.db4o.events.*;

public interface ClientConnection {
	
	/**
	 * @sharpen.property
	 */
	public String name();

	/**
	 * @sharpen.event MessageEventArgs
	 */
	public Event4<MessageEventArgs> messageReceived();
}
