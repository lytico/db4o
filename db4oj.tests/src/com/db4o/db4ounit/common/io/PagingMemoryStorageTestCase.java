/* Copyright (C) 2011 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.io;

import com.db4o.io.*;

import db4ounit.*;

public class PagingMemoryStorageTestCase implements TestCase {
	
	private static final byte[] DATA = new byte[] {1, 2, 3, 4};

	public void test() {
		PagingMemoryStorage storage = new PagingMemoryStorage() {
			
			@Override
			protected Bin produceBin(BinConfiguration config, int pageSize) {
				Bin bin = super.produceBin(config, pageSize);
				bin.write(0, DATA , DATA.length);
				return bin;
			}
			
		};
		
		Bin testBin = storage.open(new BinConfiguration("", true, 0, false));
		Assert.areEqual(DATA.length, testBin.length());
		int actualLength = (int) testBin.length();
		
		byte[] read = new byte[actualLength];
		testBin.read(0, read, actualLength);
		ArrayAssert.areEqual(DATA, read);		
	}
}
