/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.refactor;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.io.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ClassRenameByConfigExcludingOldClassTestCase implements TestLifeCycle {

	private static final String DB_PATH = "";
	private static final int NUM_ITEMS = 10;

	public static class OldItem {
		public int _id;
		
		public OldItem(int id) {
			_id = id;
		}
	}
	
	public static class NewItem {
		public int _id;
	}

	private Storage storage;

	public void test() {
		EmbeddedConfiguration config = config();
		config.common().objectClass(OldItem.class).rename(NewItem.class.getName());
		config.common().reflectWith(new ExcludingReflector(OldItem.class));
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config, DB_PATH);
		assertExtentSize(0, OldItem.class, db);
		assertExtentSize(NUM_ITEMS, NewItem.class, db);
		db.close();
	}

	private void assertExtentSize(int expectedCount, Class extent,
			EmbeddedObjectContainer db) {
		Query query = db.query();
		query.constrain(db.ext().reflector().forName(extent.getName()));
		ObjectSet<Object> result = query.execute();
		Assert.areEqual(expectedCount, result.size());
	}
	
	@Override
	public void setUp() throws Exception {
		storage = new MemoryStorage();
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config(), DB_PATH);
		for (int i = 0; i < NUM_ITEMS; i++) {
			db.store(new OldItem(i));
		}
		db.close();
	}

	@Override
	public void tearDown() throws Exception {
	}

	private EmbeddedConfiguration config() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(storage);
		return config;
	}
}
