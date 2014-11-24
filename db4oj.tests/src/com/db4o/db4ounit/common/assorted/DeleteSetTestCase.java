/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeleteSetTestCase extends AbstractDb4oTestCase {

	public static void main(String[] args) {
		new DeleteSetTestCase().runAll();
	}

	public static class Item {
		public Item() {

		}

		public Item(int v) {
			value = v;
		}

		public int value;
	}

	protected void store() throws Exception {
		store(new Item(1));
	}

	public void testDeleteStore() throws Exception {
		Object item = retrieveOnlyInstance(Item.class);
		db().delete(item);
		db().store(item);
		db().commit();
		assertOccurrences(Item.class, 1);
	}

	public void testDeleteStoreStore() throws Exception {
		Item item = (Item) retrieveOnlyInstance(Item.class);
		db().delete(item);
		item.value = 2;
		db().store(item);
		item.value = 3;
		db().store(item);
		db().commit();
		assertOccurrences(Item.class, 1);
		item = (Item) retrieveOnlyInstance(Item.class);
		Assert.areEqual(3, item.value);
	}

}
