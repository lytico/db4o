/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

/**
 * @exclude
 */
public class SynchronizedBin extends BinDecorator {

	public SynchronizedBin(Bin bin) {
	    super(bin);
    }
	
	@Override
	public void close() {
	    synchronized(_bin) {
	    	super.close();
	    }
	}
	
	@Override
	public long length() {
		synchronized(_bin) {
			return super.length();
		}
	}
	
	@Override
	public int read(long position, byte[] buffer, int bytesToRead) {
		synchronized(_bin) {
			return super.read(position, buffer, bytesToRead);
		}
	}
	
	@Override
	public void write(long position, byte[] bytes, int bytesToWrite) {
		synchronized(_bin) {
			super.write(position, bytes, bytesToWrite);
		}
	}
	
	@Override
	public void sync() {
		synchronized(_bin) {
			super.sync();
		}
	}
	
	@Override
	public void sync(Runnable runnable) {
		synchronized(_bin) {
			super.sync(runnable);
		}
	}
}
