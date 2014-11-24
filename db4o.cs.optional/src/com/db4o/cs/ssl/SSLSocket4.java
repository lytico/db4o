/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.cs.ssl;

import java.io.*;

import javax.net.ssl.SSLSocketFactory;

import com.db4o.cs.foundation.*;

import decaf.*;

@decaf.Ignore(unlessCompatible=Platform.JMX)
public class SSLSocket4 extends NetworkSocketBase {

	private SSLSocketFactory _factory;

	public SSLSocket4(String hostName, int port, SSLSocketFactory factory) throws IOException {
		super(factory.createSocket(hostName, port), hostName);
		_factory = factory;
	}

	@Override
	protected Socket4 createParallelSocket(String hostName, int port) throws IOException {
		return new SSLSocket4(hostName, port, _factory);
	}

}
