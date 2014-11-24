package com.db4o.cs.internal;

import com.db4o.events.*;

public class ClientConnectionEventArgs extends EventArgs {
	
	private final ClientConnection _connection;

	public ClientConnectionEventArgs(ClientConnection connection) {
	    _connection = connection;
    }
	
	/**
	 * @sharpen.property
	 */
	public ClientConnection connection() {
		return _connection;
	}
}
