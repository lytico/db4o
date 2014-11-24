/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.cs.internal;

import java.io.*;

import com.db4o.cs.foundation.*;
import com.db4o.ext.*;

public class Socket4Adapter {

	private final Socket4 _delegate;
	
	public Socket4Adapter(Socket4 delegate) {
		_delegate = delegate;
	}

	public Socket4Adapter(Socket4Factory socketFactory, String hostName, int port) {
		try {
			_delegate = socketFactory.createSocket(hostName, port);
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}

	public void close() throws Db4oIOException {
		try {
			_delegate.close();
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}

    public void flush() throws Db4oIOException {
		try {
			_delegate.flush();
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}   

	public boolean isConnected() {
		return _delegate.isConnected();
	}

	public Socket4Adapter openParalellSocket() throws Db4oIOException {
		try {
			return new Socket4Adapter(_delegate.openParallelSocket());
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}

	public int read(byte[] buffer, int bufferOffset, int byteCount)
			throws Db4oIOException {
		try {
			return _delegate.read(buffer, bufferOffset, byteCount);
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}

	public void setSoTimeout(int timeout) {
		_delegate.setSoTimeout(timeout);
	}

	public void write(byte[] bytes, int offset, int count)
			throws Db4oIOException {
		try {
			_delegate.write(bytes, offset, count);
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}

	public void write(byte[] bytes) throws Db4oIOException {
		try {
			_delegate.write(bytes, 0, bytes.length);
		} catch (IOException e) {
			throw new Db4oIOException(e);
		}
	}

	@Override
	public String toString() {
		return _delegate.toString();
	}
}
