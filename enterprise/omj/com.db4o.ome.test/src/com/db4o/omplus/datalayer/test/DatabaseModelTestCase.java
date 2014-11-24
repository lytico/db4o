/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.datalayer.test;

import static org.junit.Assert.*;

import org.junit.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.io.*;
import com.db4o.omplus.datalayer.*;
import com.db4o.omplus.datalayer.propertyViewer.classProperties.*;
import com.db4o.reflect.jdk.*;

public class DatabaseModelTestCase {

	private static final String DB_NAME = "inmem.db4o";

	@SuppressWarnings("unused")
	private static class Item {
		private int id;
		private String name;

		public Item(int id, String name) {
			this.id = id;
			this.name = name;
		}
	}
	
	@Test
	public void testPropertiesAreUpdated() {
		DatabaseModel model = new DatabaseModel();
		MemoryStorage storage = new MemoryStorage();
		store(storage);		
		assertIndexed(model, storage, false);
		index(storage);		
		assertIndexed(model, storage, true);
	}

	private void store(MemoryStorage storage) {
		EmbeddedObjectContainer db = db(storage);
		db.store(new Item(42, "foo"));
		db.close();
	}

	private void index(MemoryStorage storage) {
		EmbeddedObjectContainer db = db(storage);
		StoredClass clazz = db.ext().storedClass(Item.class);
		clazz.storedField("id", Integer.TYPE).createIndex();
		clazz.storedField("name", String.class).createIndex();
		db.close();
	}

	private void assertIndexed(DatabaseModel model, MemoryStorage storage, boolean indexed) {
		EmbeddedObjectContainer db = db(storage);
		model.connect(db, DB_NAME);		
		ClassProperties props = model.props().getClassProperties(Item.class.getName());
		assertEquals(1, props.getNumberOfObjects());
		FieldProperties[] fields = props.getFields();
		assertEquals(2, fields.length);
		for (FieldProperties fieldProperties : fields) {
			assertEquals(indexed, fieldProperties.isIndexed());
		}
		model.disconnect();
	}

	private EmbeddedObjectContainer db(MemoryStorage storage) {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(storage);
		config.common().reflectWith(new JdkReflector(Item.class.getClassLoader()));
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config, DB_NAME);
		return db;
	}

}
