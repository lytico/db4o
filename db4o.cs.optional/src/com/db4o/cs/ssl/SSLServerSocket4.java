/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.cs.ssl;

import java.io.*;
import java.net.*;

import javax.net.ssl.*;

import com.db4o.cs.foundation.*;

import decaf.*;

@decaf.Ignore(unlessCompatible=Platform.JMX)
public class SSLServerSocket4 extends NetworkServerSocketBase {
	private ServerSocket _socket;

	public SSLServerSocket4(int port, SSLServerSocketFactory factory) throws IOException {
		_socket = factory.createServerSocket(port);
	}

	@Override
	protected ServerSocket socket() {
		return _socket;
	}

}
