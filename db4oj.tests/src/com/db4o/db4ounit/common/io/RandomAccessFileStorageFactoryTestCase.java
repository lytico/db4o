/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.io;

import com.db4o.db4ounit.common.api.*;
import com.db4o.io.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class RandomAccessFileStorageFactoryTestCase extends TestWithTempFile {
	
	private final Storage subject = Db4oUnitPlatform.newPersistentStorage();
	
	public void testExistsWithUnexistentFile() {
		Assert.isFalse(subject.exists(tempFile()));
	}
	
	public void testExistsWithZeroLengthFile() {
		final Bin storage = subject.open(new BinConfiguration(tempFile(), false, 0, false));
		storage.close();
		Assert.isFalse(subject.exists(tempFile()));
	}	

}
