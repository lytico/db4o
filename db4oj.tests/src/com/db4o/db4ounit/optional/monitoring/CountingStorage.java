/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import com.db4o.io.*;

class CountingStorage extends StorageDecorator {
	
	private int _numberOfSyncCalls;
	
	private int _numberOfBytesRead;
	
	private int _numberOfBytesWritten;

	private int _numberOfReadCalls;
	
	private int _numberOfWriteCalls;


	public CountingStorage(Storage storage) {
		super(storage);
	}
	
	public int numberOfSyncCalls() {
		return _numberOfSyncCalls;
	}
	
	public int numberOfBytesRead() {
		return _numberOfBytesRead;
	}
	
	public int numberOfBytesWritten() {
		return _numberOfBytesWritten;
	}
	
	public int numberOfReadCalls() {
		return _numberOfReadCalls;
	}
	
	public int numberOfWriteCalls() {
		return _numberOfWriteCalls;
	}
	
	@Override
	protected Bin decorate(BinConfiguration config, Bin bin) {
		return new BinDecorator(bin) {


			@Override
			public void sync() {
				++_numberOfSyncCalls;
				super.sync();
			}
			
			@Override
			public void sync(Runnable runnable) {
				++_numberOfSyncCalls;
				super.sync(runnable);
			}
			
			@Override
			public int read(long position, byte[] bytes, int bytesToRead) {
				int bytesRead = super.read(position, bytes, bytesToRead);
				_numberOfBytesRead += bytesRead;
				++ _numberOfReadCalls;
				return bytesRead;
			}
			
			@Override
			public void write(long position, byte[] bytes, int bytesToWrite) {
				_numberOfBytesWritten += bytesToWrite;
				++ _numberOfWriteCalls;
				super.write(position, bytes, bytesToWrite);
			}
			
			
		};
	}
	
}