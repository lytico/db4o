/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.io;

import java.io.*;

/**
 * base class for IoAdapters that delegate to other IoAdapters (decorator pattern)
 */
public abstract class VanillaIoAdapter extends IoAdapter {
    
    protected IoAdapter _delegate;
    
    public VanillaIoAdapter(IoAdapter delegateAdapter){
        _delegate = delegateAdapter;
    }
    
    protected VanillaIoAdapter(IoAdapter delegateAdapter, String path, boolean lockFile, long initialLength) throws IOException {
        _delegate = delegateAdapter.open(path, lockFile, initialLength);
    }

    public void close() throws IOException {
        _delegate.close();
    }

    public void delete(String path) {
    	_delegate.delete(path);
    }
    
    public boolean exists(String path) {
    	return _delegate.exists(path);
    }
    
    public long getLength() throws IOException {
        return _delegate.getLength();
    }

    public int read(byte[] bytes, int length) throws IOException {
        return _delegate.read(bytes, length);
    }

    public void seek(long pos) throws IOException {
        _delegate.seek(pos);
    }

    public void sync() throws IOException {
        _delegate.sync();
    }

    public void write(byte[] buffer, int length) throws IOException {
        _delegate.write(buffer, length);
    }

}
