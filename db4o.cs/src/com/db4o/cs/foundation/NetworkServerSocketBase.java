/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.cs.foundation;

import java.io.*;
import java.net.*;

public abstract class NetworkServerSocketBase implements ServerSocket4 {

	protected abstract ServerSocket socket();

	public void setSoTimeout(int timeout) {
	    try {
	        socket().setSoTimeout(timeout);
	    } catch (SocketException e) {
	        e.printStackTrace();
	    }
	}

	public int getLocalPort() {
	    return socket().getLocalPort();
	}

	public Socket4 accept() throws IOException {
	    Socket sock = socket().accept();
	    // TODO: check connection permissions here
	    return new NetworkSocket(sock);
	}

	public void close() throws IOException {
		socket().close();
	}
	
}