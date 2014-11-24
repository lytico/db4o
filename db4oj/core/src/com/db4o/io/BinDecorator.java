/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

/**
 * Wrapper baseclass for all classes that wrap Bin.
 * Each class that adds functionality to a Bin can
 * extend this class to allow db4o to access the 
 * delegate instance with {@link StorageDecorator#decorate(BinConfiguration, Bin)}.
 */
public class BinDecorator implements Bin {

	protected final Bin _bin;

	/**
	 * Default constructor.
	 * @param bin the {@link Bin} that is to be wrapped.
	 */
	public BinDecorator(Bin bin) {
		_bin = bin;
	}

	/**
	 * closes the BinDecorator and the underlying {@link Bin}.
	 */
	public void close() {
    	_bin.close();
    }

	/**
	 * @see Bin#length()  
	 */
	public long length() {
		return _bin.length();
    }

	/**
	 * @see Bin#read(long, byte[], int)
	 */
	public int read(long position, byte[] bytes, int bytesToRead) {
    	return _bin.read(position, bytes, bytesToRead);
    }

	/**
	 * @see Bin#sync()
	 */
	public void sync() {
		_bin.sync();
    }
	
	/**
	 * @see Bin#syncRead(long, byte[], int)
	 */
	public int syncRead(long position, byte[] bytes, int bytesToRead) {
		return _bin.syncRead(position, bytes, bytesToRead);
	}

	/**
	 * @see Bin#write(long, byte[], int)
	 */
	public void write(long position, byte[] bytes, int bytesToWrite) {
    	_bin.write(position, bytes, bytesToWrite);
    }

	public void sync(Runnable runnable) {
		_bin.sync(runnable);
	}

}