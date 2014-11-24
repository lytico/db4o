/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import java.io.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ObjectUpdateFileSizeTestCase extends AbstractDb4oTestCase implements OptOutMultiSession, OptOutDefragSolo{

	public static void main(String[] args) {
		new ObjectUpdateFileSizeTestCase().runAll();
	}
	
	public static class Item{
		public String _name;
		
		public Item(String name){
			_name = name;
		}
	}

	protected void store() throws Exception {
		Item item = new Item("foo");
		store(item);
	}

	public void testFileSize() throws Exception {
		warmUp();
		assertFileSizeConstant();	
	}

	private void assertFileSizeConstant() throws Exception {
		
		long beforeUpdate = dbSize();
		
		for (int j = 0; j < 10; j++) {
			
			defragment();
			
			for (int i = 0; i < 15; ++i) {
				updateItem();
			}
			defragment();
			long afterUpdate = dbSize();
			
			/*
			 * FIXME: the database file size is uncertain? 
			 * We met similar problem before.
			 */
			Assert.isSmallerOrEqual(30, afterUpdate - beforeUpdate);
		}		
		
	}

	private void warmUp() throws Exception, IOException {
		for (int j = 0; j < 3; j++) {
			for (int i = 0; i < 3; ++i) {
				updateItem();
				db().commit();
				defragment();
			}
		}
	}

	private void updateItem() throws Exception, IOException {
		Item item = retrieveOnlyInstance(Item.class);
		store(item);
		db().commit();
	}
	
	private long dbSize() {
		return db().systemInfo().totalSize();
	}

}
