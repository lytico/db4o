/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;

import db4ounit.extensions.*;

public class CascadeOnDeleteHierarchyTestCase extends AbstractDb4oTestCase {

	public static void main(String[] args) {
		new CascadeOnDeleteHierarchyTestCase().runAll();
	}

	public static class Item {

	}

	public static class SubItem extends Item {
		
		public Data data;

		public SubItem() {
			data = new Data();
		}
	}

	public static class Data {
	}

	protected void configure(Configuration config) throws Exception {
		config.objectClass(Item.class).cascadeOnDelete(true);
		config.objectClass(SubItem.class);
		super.configure(config);
	}

	protected void store() throws Exception {
		store(new SubItem());
	}

	public void test() throws Exception {
		SubItem item = (SubItem) retrieveOnlyInstance(SubItem.class);
		db().delete(item);
		assertOccurrences(Data.class, 0);
		db().commit();
		assertOccurrences(Data.class, 0);
	}
	
	public void testMultipleStoreCalls(){
		SubItem item = retrieveOnlyInstance(SubItem.class);
		store(item);
		assertOccurrences(Data.class, 1);
	}
	
	
}
