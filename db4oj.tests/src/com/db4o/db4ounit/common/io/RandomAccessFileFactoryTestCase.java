/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.io;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.io.*;

import db4ounit.*;


public class RandomAccessFileFactoryTestCase extends TestWithTempFile{
	
	public void testLockDatabaseFileFalse() throws IOException{
		ObjectContainer container = openObjectContainer(false);
		RandomAccessFile raf = RandomAccessFileFactory.newRandomAccessFile(tempFile(), false, false);
        byte[] bytes = new byte[1];
	    raf.read(bytes);
		raf.close();
		container.close();
	}

	public void testLockDatabaseFileTrue() throws IOException{
		ObjectContainer container = openObjectContainer(true);
		if(! Platform4.needsLockFileThread()){
			Assert.expect(DatabaseFileLockedException.class, new CodeBlock() {
				public void run() throws Throwable {
					RandomAccessFileFactory.newRandomAccessFile(tempFile(), false, true);
				}
			});
		}
		container.close();
	}

	public void testReadOnlyLocked() throws IOException{
		final byte[] bytes = new byte[1];
		final RandomAccessFile raf = RandomAccessFileFactory.newRandomAccessFile(tempFile(), true, true);
		Assert.expect(IOException.class, new CodeBlock() {
			public void run() throws Throwable {
				raf.write(bytes);
			}
		});
		raf.close();
	}
	
	public void testReadOnlyUnLocked() throws IOException{
		final byte[] bytes = new byte[1];
		final RandomAccessFile raf = RandomAccessFileFactory.newRandomAccessFile(tempFile(), true, false);
		Assert.expect(IOException.class, new CodeBlock() {
			public void run() throws Throwable {
				raf.write(bytes);
			}
		});
		raf.close();
	}

	
	private ObjectContainer openObjectContainer(boolean lockDatabaseFile) {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().lockDatabaseFile(lockDatabaseFile);
		return Db4oEmbedded.openFile(config, tempFile());
	}
	



}
