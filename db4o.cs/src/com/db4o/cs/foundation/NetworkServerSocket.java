/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.foundation;

import java.io.*;
import java.net.*;

public class NetworkServerSocket extends NetworkServerSocketBase {

	private ServerSocket _socket;
	
    public NetworkServerSocket(int port) throws IOException {
        _socket = new ServerSocket(port);
    }

	@Override
	protected ServerSocket socket() {
		return _socket;
	}

}
