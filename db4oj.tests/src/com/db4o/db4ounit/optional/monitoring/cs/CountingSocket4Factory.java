/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.optional.monitoring.cs;

import java.io.*;
import java.util.*;

import com.db4o.cs.foundation.*;

public class CountingSocket4Factory implements Socket4Factory {

	public CountingSocket4Factory(Socket4Factory socketFactory) {		
		_socketFactory = socketFactory;
	}

	public ServerSocket4 createServerSocket(int port) throws IOException {
		_serverSocket = new CountingServerSocket4(_socketFactory.createServerSocket(port));
		return _serverSocket;
	}

	public Socket4 createSocket(String hostName, int port) throws IOException {
		CountingSocket4 socket = new CountingSocket4(_socketFactory.createSocket(hostName, port));
		_sockets.add(socket);
		return socket;
	}

	public List<CountingSocket4> countingSockets() {
		return _sockets;
	}
	
	public List<CountingSocket4> connectedClients() {
		return _serverSocket.connectedClients();
	}
	
	public void resetCounters() {
		for (CountingSocket4 socket : connectedClients()) {
			socket.resetCount();
		}
	}
	
	private CountingServerSocket4 _serverSocket;
	private Socket4Factory _socketFactory;
	private List<CountingSocket4> _sockets = new ArrayList<CountingSocket4>();
}
