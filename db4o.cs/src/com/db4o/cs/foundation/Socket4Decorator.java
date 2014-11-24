/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.cs.foundation;

import java.io.IOException;

/**
 * @since 7.12
 */
public class Socket4Decorator implements Socket4 {
	public Socket4Decorator(Socket4 socket) {	
		_socket = socket;
	}

	public void close() throws IOException {
		_socket.close();
	}

	public void flush() throws IOException {
		_socket.flush();
	}

	public boolean isConnected() {
		return _socket.isConnected();
	}

	public Socket4 openParallelSocket() throws IOException {
		return _socket.openParallelSocket();
	}

	public int read(byte[] buffer, int offset, int count) throws IOException {
		return _socket.read(buffer, offset, count);
	}

	public void setSoTimeout(int timeout) {
		_socket.setSoTimeout(timeout);
	}

	public void write(byte[] bytes, int offset, int count) throws IOException {
		_socket.write(bytes, offset, count);
	}
	
	@Override
	public String toString() {
		return _socket.toString();
	}
	
	protected Socket4 _socket;
}
