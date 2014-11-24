/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import java.io.*;

import com.db4o.cs.foundation.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class MonitoredSocket4Factory implements Socket4Factory {
	
	private final Socket4Factory _socketFactory;
	
	public MonitoredSocket4Factory(Socket4Factory socketFactory) {
		_socketFactory = socketFactory;
	}

	public ServerSocket4 createServerSocket(final int port) throws IOException {
		return new MonitoredServerSocket4(_socketFactory.createServerSocket(port));
	}
	
	public Socket4 createSocket(String hostName, int port) throws IOException {
		return new MonitoredClientSocket4(_socketFactory.createSocket(hostName, port));
	}

}
