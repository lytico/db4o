/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.tp;

import com.db4o.config.*;
import com.db4o.ta.*;

import db4ounit.extensions.*;
import db4ounit.mocking.*;

public class RollbackStrategyTestCase extends AbstractDb4oTestCase {
	
	private final RollbackStrategyMock _mock = new RollbackStrategyMock();
	
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentPersistenceSupport(_mock));
	}
	
	public void testRollbackStrategyIsCalledForChangedObjects() {
		Item item1 = storeItem("foo");
		Item item2 = storeItem("bar");
		storeItem("baz");
		
		change(item1);
		change(item2);
		
		_mock.verify(new MethodCall[0]);
		
		db().rollback();
		
		_mock.verifyUnordered(new MethodCall[] {
			new MethodCall("rollback", db(), item1),
			new MethodCall("rollback", db(), item2),
		});
		
	}

	private void change(Item item) {
		item.setName(item.getName() + "*");
	}

	private Item storeItem(String name) {
		final Item item = new Item(name);
		store(item);
		return item;
	}
	
	public static void main(String []args) {
		new RollbackStrategyTestCase().runAll();
	}

}
