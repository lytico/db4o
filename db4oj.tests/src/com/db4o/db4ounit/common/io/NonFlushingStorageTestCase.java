package com.db4o.db4ounit.common.io;

import java.io.*;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.io.*;

import db4ounit.*;
import db4ounit.mocking.*;

public class NonFlushingStorageTestCase implements TestCase {
	
	public void test() {
		final MockBin mock = new MockBin();
		
		BinConfiguration binConfig = new BinConfiguration("uri", true, 42L, false);
		
		final Bin storage = new NonFlushingStorage(new Storage() {
			public boolean exists(String uri) {
				throw new NotImplementedException();
            }

			public Bin open(BinConfiguration config)
                    throws Db4oIOException {
				mock.record(new MethodCall("open", config));
				return mock;
            }

			public void delete(String uri) throws IOException {
				throw new NotImplementedException();
			}

			public void rename(String oldUri, String newUri) throws IOException {
				throw new NotImplementedException();
			}
			
		}).open(binConfig);
		
		final byte[] buffer = new byte[5];
		storage.read(1, buffer, 4);
		storage.write(2, buffer, 3);
		mock.returnValueForNextCall(42);
		Assert.areEqual(42, mock.length());
		storage.sync();
		storage.close();
		
		mock.verify(
			new MethodCall("open", binConfig),
			new MethodCall("read", 1L, buffer, 4),
			new MethodCall("write", 2L, buffer, 3),
			new MethodCall("length"),
			new MethodCall("close")
		);
	}

}
