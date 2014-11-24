/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.foundation.io.*;

import db4ounit.*;

/**
 * @sharpen.partial
 */
public class DbPathDoesNotExistTestCase extends Db4oTestWithTempFile {
	
	public void test(){
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				Db4oEmbedded.openFile(DbPathDoesNotExistTestCase.this.newConfiguration(), nonExistingFilePath());
			}
		});
		
	}

	/**
	 * @sharpen.ignore
	 */
	private String nonExistingFilePath() {
		String tempPath = Path4.getTempPath();
		return Path4.combine(tempPath, "/folderdoesnotexistneverever/filedoesnotexist.db4o");
	}

}
