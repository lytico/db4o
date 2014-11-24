/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.io.*;
import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class KnownClassesIndexTestCase implements TestLifeCycle {

	private static final String DB_PATH = "inmem";
	private Storage _storage = new MemoryStorage();
	
	public static class WithIndex {
		public int _id;

		public WithIndex(int id) {
			_id = id;
		}
	}

	@Override
	public void setUp() throws Exception {
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config(), DB_PATH);
		db.store(new WithIndex(42));
		db.close();
	}

	private EmbeddedConfiguration config() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.common().objectClass(WithIndex.class).objectField("_id").indexed(true);
		config.file().storage(_storage);
		return config;
	}

	@Override
	public void tearDown() throws Exception {
	}

    public void testIndexInfoAvailableAfterInfoGathering() {
    	EmbeddedConfiguration config = config();
    	config.common().reflectWith(new ExcludingReflector(WithIndex.class));
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config, DB_PATH);
		try {
	    	scanThroughKnownClassesInfo(db);
	    	assertHasIndexInfo(db);
		}
		finally {
			db.close();
		}
    }

    private void scanThroughKnownClassesInfo(ObjectContainer db)
    {
    	for(ReflectClass clazz : db.ext().knownClasses()) {
    		for(ReflectField field: clazz.getDeclaredFields()) {
    			field.getName();
    			field.getFieldType();
    		}
    	}
    }


    private void assertHasIndexInfo(ObjectContainer db) {
    	for(StoredClass sc : db.ext().storedClasses()) {
    		if(!sc.getName().equals(WithIndex.class.getName())) {
    			continue;
    		}
    		for(StoredField sf : sc.getStoredFields()) {
    			if(sf.hasIndex()) {
    				return;
    			}
    		}
    		Assert.fail("no index found");
    	}
    }

}
