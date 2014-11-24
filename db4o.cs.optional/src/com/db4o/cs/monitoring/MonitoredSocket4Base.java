/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import java.io.*;

import com.db4o.cs.foundation.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
abstract class MonitoredSocket4Base extends Socket4Decorator {
	public MonitoredSocket4Base(Socket4 socket) {
		super(socket);
	}

	public void write(byte[] bytes, int offset, int count) throws IOException {
		bean().notifyWrite(count);
		super.write(bytes, offset, count);
	}
	
	@Override
	public int read(byte[] buffer, int offset, int count) throws IOException {
		int bytesReceived = super.read(buffer, offset, count);
		bean().notifyRead(bytesReceived);
		
		return bytesReceived;
	}
	
	protected abstract Networking bean();
}