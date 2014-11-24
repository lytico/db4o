/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.datalayer.test;

import static org.junit.Assert.*;

import org.junit.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.io.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.propertyViewer.*;
import com.db4o.omplus.datalayer.propertyViewer.classProperties.*;
import com.db4o.reflect.jdk.*;

public class ConfigureIndexTestCase {

	private static final String DB_NAME = "inmem.db4o";

	@SuppressWarnings("unused")
	private static class Item {
		private final int id;
		private final String name;

		public Item(int id, String name) {
			this.id = id;
			this.name = name;
		}
	}
	
	@Test
	public void testIntStringIndex() {
		MemoryStorage storage = new MemoryStorage();
		store(storage);		
		assertIndexed(storage, true);
		assertIndexed(storage, false);
	}

	private void assertIndexed(MemoryStorage storage, boolean index) {
		DatabaseModel dbModel = new DatabaseModel();
		dbModel.connect(db(storage), DB_NAME);
		ClassProperties props = dbModel.props().getClassProperties(Item.class.getName());
		for (FieldProperties field : props.getFields()) {
			assertEquals(!index, field.isIndexed());
			field.setIndexed(index);
		}
		new ConfigureIndex(dbModel).createIndex(props);
		dbModel.disconnect();
		
		EmbeddedObjectContainer db = db(storage);
		StoredField[] fields = db.ext().storedClass(Item.class).getStoredFields();
		for (StoredField field : fields) {
			assertEquals(index, field.hasIndex());
		}
		db.close();
	}
	
	private void store(MemoryStorage storage) {
		EmbeddedObjectContainer db = db(storage);
		db.store(new Item(42, "foo"));
		db.close();
	}

	private EmbeddedObjectContainer db(MemoryStorage storage) {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(storage);
		config.common().reflectWith(new JdkReflector(Item.class.getClassLoader()));
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config, DB_NAME);
		return db;
	}
}
