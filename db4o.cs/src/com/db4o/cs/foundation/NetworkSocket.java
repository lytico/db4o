/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.foundation;

import java.io.*;
import java.net.*;

public class NetworkSocket extends NetworkSocketBase {

    public NetworkSocket(String hostName, int port) throws IOException {
    	super(new Socket(hostName, port), hostName);
    }

	public NetworkSocket(Socket socket) throws IOException {
		super(socket);
	}

	@Override
	protected Socket4 createParallelSocket(String hostName, int port) throws IOException {
		return new NetworkSocket(hostName, port);
	}

}
