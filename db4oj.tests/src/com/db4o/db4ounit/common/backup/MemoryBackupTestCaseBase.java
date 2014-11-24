/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.backup;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.internal.*;
import com.db4o.io.*;

import db4ounit.*;

public abstract class MemoryBackupTestCaseBase extends TestWithTempFile {

	public static class Item {
		public int _id;
	
		public Item(int id) {
			_id = id;
		}
	}

	private static final String DB_PATH = "database";
	private static final int NUM_ITEMS = 10;
	 
	public void testMemoryBackup() throws Exception {
		LocalObjectContainer origDb = (LocalObjectContainer) Db4oEmbedded.openFile(config(origStorage()), DB_PATH);
		store(origDb);
		backup(origDb, tempFile());
		origDb.close();
	
		ObjectContainer backupDb = Db4oEmbedded.openFile(config(backupStorage()), tempFile());
		ObjectSet<Item> result = backupDb.query(Item.class);
		Assert.areEqual(NUM_ITEMS, result.size());
		backupDb.close();
		backupStorage().delete(tempFile());
	}

	protected abstract void backup(LocalObjectContainer origDb, String backupPath);

	protected abstract Storage backupStorage();

	protected abstract Storage origStorage();

	private void store(LocalObjectContainer origDb) {
		for(int itemId = 0; itemId < NUM_ITEMS; itemId++) {
			origDb.store(new Item(itemId));
		}
		origDb.commit();
	}

	private EmbeddedConfiguration config(Storage storage) {
		EmbeddedConfiguration origConfig = Db4oEmbedded.newConfiguration();
		origConfig.file().storage(storage);
		return origConfig;
	}

}