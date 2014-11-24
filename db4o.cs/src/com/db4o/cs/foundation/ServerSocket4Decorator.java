/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.foundation;

import java.io.IOException;

/**
 * @since 7.12
 */
public class ServerSocket4Decorator implements ServerSocket4 {

	public ServerSocket4Decorator(ServerSocket4 serverSocket) {
		_serverSocket = serverSocket;
	}
	
	public Socket4 accept() throws IOException {
		return _serverSocket.accept();
	}

	public void close() throws IOException {
		_serverSocket.close();
	}

	public int getLocalPort() {
		return _serverSocket.getLocalPort();
	}

	public void setSoTimeout(int timeout) {
		_serverSocket.setSoTimeout(timeout);
	}
	
	protected ServerSocket4 _serverSocket;
}
