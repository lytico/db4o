/* Copyright (C) 2004 - 2008 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeactivateTestCase extends AbstractDb4oTestCase  {
	protected void store() throws Exception {
		db().store(new Item("foo", new Item("bar", null)));
	}
	
	public void test() {		
		Query query = newQuery();
		query.descend("_name").constrain("foo");
		
		ObjectSet results = query.execute();
		Assert.areEqual(1, results.size());
		
		Item item1 = (Item) results.next();	
		Item item2 = item1._child;
		
		Assert.isTrue(db().isActive(item1));
		Assert.isTrue(db().isActive(item2));
		
		db().deactivate(item1);
		
		Assert.isFalse(db().isActive(item1));
		Assert.isTrue(db().isActive(item2));
	}
	
	public static void main(String []args) {
		new DeactivateTestCase().runAll();
	}
	
	public static class Item {
		public Item _child;
		public String _name;
		
		public Item(String name, Item child) {
			_name = name;
			_child = child;
		}
	}
}
