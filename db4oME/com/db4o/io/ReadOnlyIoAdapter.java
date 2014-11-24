package com.db4o.io;

import java.io.*;

public class ReadOnlyIoAdapter extends VanillaIoAdapter {
	public ReadOnlyIoAdapter(IoAdapter delegateAdapter) {
		super(delegateAdapter);
	}

    protected ReadOnlyIoAdapter(IoAdapter delegateAdapter, String path, boolean lockFile, long initialLength) throws IOException {
        super(delegateAdapter, path, lockFile, initialLength);
    }

	public IoAdapter open(String path, boolean lockFile, long initialLength) throws IOException {
		return new ReadOnlyIoAdapter(_delegate,path,lockFile,initialLength);
	}

	public void write(byte[] buffer, int length) throws IOException {
		// do nothing
	}
}
