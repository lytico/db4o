/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import java.io.*;

import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.internal.*;

import db4ounit.*;

/**
 * @exclude
 */
public class Platform4TestCase extends TestWithTempFile{
	
	/**
	 * @sharpen.remove
	 */
	public void testLockingReadOnlyFileFails() throws FileNotFoundException{
		RandomAccessFile raf = null;
		try {
			raf = new RandomAccessFile(tempFile(), "r");
			final RandomAccessFile finalRaf = raf;
			if(! Platform4.needsLockFileThread()){
				Assert.expect(DatabaseFileLockedException.class, new CodeBlock() {
					public void run() throws Throwable {
						Platform4.lockFile(tempFile(), finalRaf);
					}
				});
			}
		} finally {
			if (raf != null){
				try {
					raf.close();
				} catch (IOException e) {
				}
			}
		}
	}

}
