/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.ta.collections;

import java.util.*;

import com.db4o.db4ounit.common.ta.*;

public class TPCollectionUpdateFieldIndexConsistencyTestCase extends TPFieldIndexConsistencyTestCaseBase {

	/**
	 * @sharpen.ignore
	 */
	@decaf.Ignore(decaf.Platform.JDK11)
	public static class Holder {
		public List<Item> _items = new com.db4o.collections.ActivatableArrayList<Item>();
		
		public void add(Item item) {
			_items.add(item);
		}
	}
	
	/**
	 * @sharpen.ignore
	 */
	@decaf.Ignore(decaf.Platform.JDK11)
	public void testImplicitStoreThroughCollection() {
		int id = 42;
		Item item = new Item(id);
		Holder holder = new Holder();
		store(item);
		store(holder);
		holder.add(item);
		assertItemQuery(id);
		commit();
		assertFieldIndex(id);
	}
}
