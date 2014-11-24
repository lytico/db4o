/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class StringIndexWithSuperClassTestCase extends AbstractDb4oTestCase {

	private static final String FIELD_NAME = "_name";
	private static final String FIELD_VALUE = "test";

	public static class ItemParent {
		public int _id;
	}
	
	public static class Item extends ItemParent {
		public String _name;

		public Item(String name) {
			_name = name;
		}
	}
	
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Item.class).objectField(FIELD_NAME).indexed(true);
	}
	
	protected void store() throws Exception {
		store(new Item(FIELD_VALUE));
		store(new Item(FIELD_VALUE + "X"));
	}

	public void testIndexAccess() {
		Query query = newQuery(Item.class);
		query.descend(FIELD_NAME).constrain(FIELD_VALUE);
		Assert.areEqual(1, query.execute().size());
	}
}
