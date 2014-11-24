/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.test.nativequery;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class NQPredicateFalseTestCase extends AbstractInMemoryDb4oTestCase {

	public static class Item {
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item());
	}
	
	public void testPredicateFalse() {
		ObjectSet<Item> result = db().query(new Predicate<Item>() {
			@Override
			public boolean match(Item candidate) {
				return false;
			}
		});
		Assert.isTrue(result.isEmpty());
	}
	
}
