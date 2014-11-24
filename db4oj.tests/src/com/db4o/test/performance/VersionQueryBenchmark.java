/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */
package com.db4o.test.performance;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.query.*;

public class VersionQueryBenchmark {
	
	private static final int COUNT = 10000;
	private static final String FILE_NAME = "version-benchmark.db4o";
	
	
	public static class Item {
		public Item(int id) {
			this.id = id;
		}

		int id;
		
	}

	public static void main(String[] args) {
		
		EmbeddedObjectContainer db;
		
//		deleteContainer();
//		
//		EmbeddedObjectContainer db = openContainer();
//		
//		populate(db);
//		
//		db.close();
		
		for(int i=0;i<5;i++) {
			benchmark();
		}
	}

	private static void benchmark() {
		EmbeddedObjectContainer db;
		db = openContainer();
		
		int firstId = (int) (COUNT * .1);
		int lastId = (int) (COUNT * .1 * 2.);

		long versionFirst = versionForItem(db, firstId);
		long versionLast = versionForItem(db, lastId);
		
		long before = System.currentTimeMillis();
		
		Query q = db.query();
		q.descend(VirtualField.COMMIT_TIMESTAMP).constrain(versionFirst).greater();
		q.descend(VirtualField.COMMIT_TIMESTAMP).constrain(versionLast).smaller();
		
		q.execute();
		
		long now = System.currentTimeMillis();
		
		System.out.println("Querying " + (lastId-firstId) +" objects out of " + COUNT +" took: " +(now-before) + "ms");
		
		db.close();
	}

	private static long versionForItem(EmbeddedObjectContainer db, int itemId) {
		Query q = db.query();
		q.constrain(Item.class);
		q.descend("id").constrain(itemId);
		Object item = q.execute().next();
		return db.ext().getObjectInfo(item).getCommitTimestamp();
	}

	private static void deleteContainer() {
		new File(FILE_NAME).delete();
	}

	private static EmbeddedObjectContainer openContainer() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().generateCommitTimestamps(true);
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config, FILE_NAME);
		return db;
	}

	private static void populate(EmbeddedObjectContainer db) {
		for(int i=0;i<COUNT;i++) {
			db.store(new Item(i+1));
			db.commit();
		}
	}

}
