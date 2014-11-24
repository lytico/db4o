/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
package com.db4o.cs.foundation;

import java.io.*;

public class StandardSocket4Factory implements Socket4Factory {

	public ServerSocket4 createServerSocket(int port) throws IOException {
		return new NetworkServerSocket(port);
	}

	public Socket4 createSocket(String hostName, int port) throws IOException {
		return new NetworkSocket(hostName, port);
	}

}
