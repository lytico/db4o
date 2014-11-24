/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

public class TPExplicitStoreFieldIndexConsistencyTestCase extends TPFieldIndexConsistencyTestCaseBase {

	public void testExplicitStore() {
		int id = 42;
		Item item = new Item(id);
		store(item);
		store(item);
		assertItemQuery(id);
		commit();
		assertFieldIndex(id);
	}

}
