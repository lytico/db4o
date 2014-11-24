/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.optional.monitoring.cs;

import java.io.IOException;

import com.db4o.cs.foundation.*;

public class CountingSocket4 extends Socket4Decorator {
	
	private final Object lock = new Object();
	
	public CountingSocket4(Socket4 socket) {	
		super(socket);
	}

	public void write(byte[] bytes, int offset, int count) throws IOException {
		synchronized(lock){
			super.write(bytes, offset, count);
			_bytesSent += count;
			_messagesSent++;
		}
	}

	@Override
	public int read(byte[] buffer, int offset, int count) throws IOException {
		int bytesReceived = super.read(buffer, offset, count);
		synchronized(lock){
			_bytesReceived += bytesReceived;
		}
		return bytesReceived;
	}
	
	public double bytesSent() {
		synchronized(lock){
			return _bytesSent;
		}
	}

	public double bytesReceived() {
		synchronized(lock){
			return _bytesReceived;
		}
	}

	public double messagesSent() {
		synchronized(lock){
			return _messagesSent;
		}
	}
	
	public void resetCount() {
		synchronized(lock){
			_bytesSent = 0.0;
			_bytesReceived = 0.0;
			_messagesSent = 0.0;
		}
	}
	
	private double _bytesSent;
	private double _bytesReceived;
	private double _messagesSent;
}
