/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

/**
 * Storage adapter that does not pass flush calls 
 * on to its delegate.
 * You can use this {@link Storage} for improved db4o
 * speed at the risk of corrupted database files in 
 * case of system failure.    
 */
public class NonFlushingStorage extends StorageDecorator {

	public NonFlushingStorage(Storage storage) {
		super(storage);
    }

	@Override
	protected Bin decorate(BinConfiguration config, Bin storage) {
		return new NonFlushingBin(storage);
	}
	
	private static class NonFlushingBin extends BinDecorator {

		public NonFlushingBin(Bin storage) {
			super(storage);
	    }
		
		@Override
		public void sync() {
		}
		
		@Override
		public void sync(Runnable runnable) {
			runnable.run();
		}
		
		
	}

}
