/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.cs.ssl;

import java.io.*;

import javax.net.ssl.*;

import com.db4o.cs.foundation.*;

import decaf.*;

@decaf.Ignore(unlessCompatible=Platform.JMX)
public class SSLSocketFactory implements Socket4Factory {

	private SSLContext _context;

	public SSLSocketFactory(SSLContext context) {
		_context = context;
	}
	
	public ServerSocket4 createServerSocket(int port) throws IOException {
		return new SSLServerSocket4(port, (_context == null ? (SSLServerSocketFactory)SSLServerSocketFactory.getDefault() : _context.getServerSocketFactory()));
	}

	public Socket4 createSocket(String hostName, int port) throws IOException {
		return new SSLSocket4(hostName, port, (_context == null ? (javax.net.ssl.SSLSocketFactory)javax.net.ssl.SSLSocketFactory.getDefault() : _context.getSocketFactory()));
	}

}
