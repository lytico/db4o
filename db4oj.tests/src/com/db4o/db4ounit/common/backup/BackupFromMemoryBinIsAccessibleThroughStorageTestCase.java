/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.backup;

import com.db4o.internal.*;
import com.db4o.io.*;

public class BackupFromMemoryBinIsAccessibleThroughStorageTestCase extends MemoryBackupTestCaseBase {

	protected MemoryStorage _storage = new MemoryStorage();

	@Override
	protected Storage origStorage() {
		return _storage;
	}

	@Override
	protected Storage backupStorage() {
		return _storage;
	}

	@Override
	protected void backup(LocalObjectContainer origDb, String backupPath) {
		origDb.backup(backupPath);
	}
	
}
