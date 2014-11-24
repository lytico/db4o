/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class SameChildOnDifferentParentQueryTestCase extends AbstractDb4oTestCase {

	public static class Holder {
		
		public Item _child;
		
		public Holder(Item belongs) {
			_child = belongs;
		}
	}
	
	public static class Item {
		
		public String _name;
		
		public Item(String name) {
			_name = name;
		}
	}
	
	@Override
	protected void store() throws Exception {
		
		Item unique = new Item("unique");
		Item shared = new Item("shared");

		store(new Holder(shared));
		store(new Holder(unique));
		store(new Holder(shared));
	}

	public void testUniqueResult() {
		Query query = db().query();
		query.constrain(Holder.class);
		query.descend("_child").descend("_name").constrain("unique");

		ObjectSet<Holder> result = query.execute();
		Assert.areEqual(1, result.size());
		Holder holder = result.next();
		Assert.areEqual("unique", holder._child._name);
	}
	
}