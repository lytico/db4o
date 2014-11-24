/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.ordered;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class OrderByWithNullValuesTestCase extends AbstractDb4oTestCase {

	public static class Item {
		public int _id;
		public String _name;
		
		public Item(int id, String name) {
			_id = id;
			_name = name;
		}
		
		public String name() {
			return _name;
		}
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(1, "a"));
		store(new Item(2, null));
		store(new Item(3, "b"));
		store(new Item(4, null));
	}
	
	public void testOrderByWithNullValues() {
		Query query = newQuery();
		query.constrain(Item.class);
		query.descend("_name").orderAscending();
		ObjectSet<Item> result = query.execute();
		Assert.areEqual(4, result.size());
		Assert.isNull(result.next().name());
		Assert.isNull(result.next().name());
		Assert.areEqual("a", result.next().name());
		Assert.areEqual("b", result.next().name());
	}
	
}
