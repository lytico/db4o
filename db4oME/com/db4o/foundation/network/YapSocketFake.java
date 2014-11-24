/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.foundation.network;

import java.io.*;

/**
 * Fakes a socket connection for an embedded client.
 */
public class YapSocketFake implements YapSocket {
	
	private final YapSocketFakeServer _server;

    private YapSocketFake _affiliate;
    private ByteBuffer4 _uploadBuffer;
    private ByteBuffer4 _downloadBuffer;

    public YapSocketFake(YapSocketFakeServer a_server, int timeout) {
    	_server = a_server;
        _uploadBuffer = new ByteBuffer4(timeout);
        _downloadBuffer = new ByteBuffer4(timeout);
    }

    public YapSocketFake(YapSocketFakeServer a_server, int timeout, YapSocketFake affiliate) {
        this(a_server, timeout);
        _affiliate = affiliate;
        affiliate._affiliate = this;
        _downloadBuffer = affiliate._uploadBuffer;
        _uploadBuffer = affiliate._downloadBuffer;
    }

    public void close() throws IOException {
        if (_affiliate != null) {
            YapSocketFake temp = _affiliate;
            _affiliate = null;
            temp.close();
        }
        _affiliate = null;
    }

    public void flush() {
        // do nothing
    }

    public boolean isClosed() {
        return _affiliate == null;
    }

    public int read() throws IOException {
        return _downloadBuffer.read();
    }

    public int read(byte[] a_bytes, int a_offset, int a_length) throws IOException {
        return _downloadBuffer.read(a_bytes, a_offset, a_length);
    }

    public void setSoTimeout(int a_timeout) {
        _uploadBuffer.setTimeout(a_timeout);
        _downloadBuffer.setTimeout(a_timeout);
    }

    public void write(byte[] bytes) throws IOException {
        _uploadBuffer.write(bytes);
    }
    
    public void write(byte[] bytes,int off,int len) throws IOException {
        _uploadBuffer.write(bytes, off, len);
    }

    public void write(int i) throws IOException {
        _uploadBuffer.write(i);
    }
    
    public YapSocket openParalellSocket() throws IOException {
    	return _server.openClientSocket();
    }
}
