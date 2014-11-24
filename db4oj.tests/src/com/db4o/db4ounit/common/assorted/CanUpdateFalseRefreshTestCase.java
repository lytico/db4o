/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CanUpdateFalseRefreshTestCase extends AbstractDb4oTestCase{
	
	public static class Item {
		
		public int _id;
		
		public String _name;

		public Item(int id, String name) {
			_id = id;
			_name = name;
		}

		public boolean objectCanUpdate(ObjectContainer container) {
			return false;
		}
	}
	
	protected void store() throws Exception {
		store(new Item(1, "one"));
	}
	
	public void test(){
		Item item = (Item) retrieveOnlyInstance(Item.class);
		item._name = "two";
		db().store(item);
		
		Assert.areEqual("two", item._name);
		db().refresh(item, 2);
		Assert.areEqual("one", item._name);
	}

	public static void main(String[] args) {
		new CanUpdateFalseRefreshTestCase().runSoloAndClientServer();
	}

}
