package com.db4o.db4ounit.common.soda.ordered;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TopLevelOrderExceptionTestCase extends AbstractDb4oTestCase {

	public static class Item {
	}

	@Override
	protected void store() throws Exception {
		store(new Item());
		store(new Item());
	}
	
	public void testDescending() {
		final Query query = newQuery(Item.class);
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				query.orderDescending();
			}
		});
	}

	public void testAscending() {
		final Query query = newQuery(Item.class);
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				query.orderAscending();
			}
		});
	}
}
