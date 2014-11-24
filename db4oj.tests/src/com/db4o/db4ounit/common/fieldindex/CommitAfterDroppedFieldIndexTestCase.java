/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.extensions.*;

/**
 * @sharpen.if !SILVERLIGHT
 */
public class CommitAfterDroppedFieldIndexTestCase extends Db4oClientServerTestCase {
	
	public static class Item {
		
		public int _id;

		public Item(int id){
			_id = id;
		}
		
	}

	private static final int OBJECT_COUNT = 100;
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.clientServer().prefetchIDCount(1);
		config.clientServer().batchMessages(false);
		config.bTreeNodeSize(4);
	}
	
	public void test(){
		
		for (int i = 0; i < OBJECT_COUNT; i++) {
			store(new Item(1));
		}
		
		StoredField storedField = fileSession().storedClass(Item.class).storedField("_id", null);
		storedField.createIndex();
		fileSession().commit();
		
		
		ExtObjectContainer session = openNewSession();
		
		ObjectSet<Item> allItems = session.query(Item.class);
		for (Item item : allItems) {
			item._id++;
			session.store(item);
		}
		
		// Making sure all storing has been processed.
		session.setSemaphore("anySemaphore", 0);
		
		storedField.dropIndex();
		session.commit();
		
		storedField.createIndex();
		
	}

}
