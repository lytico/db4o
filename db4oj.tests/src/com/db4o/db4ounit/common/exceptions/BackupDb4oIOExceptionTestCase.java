/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.ext.*;
import com.db4o.foundation.io.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;

public class BackupDb4oIOExceptionTestCase
	extends Db4oIOExceptionTestCaseBase
	implements OptOutInMemory {
	
	public static void main(String[] args) {
		new BackupDb4oIOExceptionTestCase().runAll();
	}
	
	private static final String BACKUP_FILE = "backup.db4o";

	protected void db4oSetupBeforeStore() throws Exception {
		super.db4oSetupBeforeStore();
		File4.delete(BACKUP_FILE);
	}

	protected void db4oTearDownBeforeClean() throws Exception {
		super.db4oTearDownBeforeClean();
		File4.delete(BACKUP_FILE);
	}
	
	public void testBackup() {
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				triggerException(true);
				db().backup(BACKUP_FILE);
			}
		});
	}
	
}
