/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class IdentityQueryForNotStoredTestCase extends AbstractDb4oTestCase{
	
	public static class Item{
		
		public String _name;
		
		public Item _child;
		
		public Item(Item child, String name) {
			_child = child;
			_name = name;
		}
	}
	
	@Override
	protected void store() throws Exception {
		Item item = new Item(null, "foo");
		store(new Item( item, "bar"));
	}
	
	public void test(){
		Query q = newQuery(Item.class);
		q.descend("_child").constrain(new Item(null, "foo")).identity();
		Assert.areEqual(0, q.execute().size());
	}

}
