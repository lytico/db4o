package com.db4o.db4ounit.common.soda;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class InterfaceFieldConstraintTestCase extends AbstractDb4oTestCase {

	private static final int ID = 42;

	public static interface IFoo {
	}

	public static class Foo implements IFoo {
		public int _id;
		
		public Foo(int id) {
			_id = id;
		}
	}
	
	@Override
	protected void store() throws Exception {
		store(new Foo(ID));
	}

	public void testInterfaceFieldQuery() {
		Query query = newQuery(IFoo.class);
		query.descend("_id").constrain(ID);
		Assert.areEqual(1, query.execute().size());
	}
}
