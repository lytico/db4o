/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.optional.monitoring.cs;

import java.io.IOException;
import java.util.*;

import com.db4o.cs.foundation.*;

public class CountingServerSocket4 implements ServerSocket4 {

	public CountingServerSocket4(ServerSocket4 serverSocket) {
		_serverSocket = serverSocket;
	}

	public Socket4 accept() throws IOException {
		CountingSocket4 socket = new CountingSocket4(_serverSocket.accept());
		_clients.add(socket);
		
		return socket;
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
	
	public List<CountingSocket4> connectedClients() {
		return _clients;
	}

	private ServerSocket4 _serverSocket;
	private List<CountingSocket4> _clients = new ArrayList<CountingSocket4>();
}
