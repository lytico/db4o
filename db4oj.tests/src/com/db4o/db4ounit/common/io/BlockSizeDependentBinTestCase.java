/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.io;

import static com.db4o.foundation.Environments.my;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

import db4ounit.*;

/**
 * @exclude
 */
public class BlockSizeDependentBinTestCase extends TestWithTempFile {
	
	public static class BlockSizeDependentStorage extends StorageDecorator{
		
		private final IntByRef _blockSize;

		public BlockSizeDependentStorage(Storage storage, IntByRef blockSize) {
			super(storage);
			_blockSize = blockSize;
		}
		
		@Override
		public Bin open(BinConfiguration config) throws Db4oIOException {
			Bin bin = super.open(config);
			
			my(BlockSize.class).register((Listener4<Integer>) bin);
			
			return bin;
		}
		
		@Override
		protected Bin decorate(BinConfiguration config, Bin bin) {
			return new BlockSizeDependentBin(bin, _blockSize);
		}
		
		private static class BlockSizeDependentBin extends BinDecorator implements Listener4<Integer> {
			
			private final IntByRef _blockSize;

			public BlockSizeDependentBin(Bin bin, IntByRef blockSize) {
				super(bin);
				_blockSize = blockSize;
			}

			public void onEvent(Integer event) {
				_blockSize.value = event;
			}
		}
	}
	
	private IntByRef _blockSize = new IntByRef();
	
	public void test(){
		int configuredBlockSize = 13;
		ObjectContainer db = Db4oEmbedded.openFile(configure(configuredBlockSize), tempFile());
		try {
			Assert.areEqual(configuredBlockSize, _blockSize.value);
		} finally {
			db.close();
		}
		
		db = Db4oEmbedded.openFile(configure(14), tempFile());
		try {
			Assert.areEqual(configuredBlockSize, _blockSize.value);
		} finally {
			db.close();
		}
	}

	private EmbeddedConfiguration configure(int configuredBlockSize) {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(new BlockSizeDependentStorage(new FileStorage(), _blockSize));
		config.file().blockSize(configuredBlockSize);
		return config;
	}

}
