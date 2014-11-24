/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.defragment;

import com.db4o.config.*;
import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class RemovedFieldDefragmentTestCase extends AbstractDb4oTestCase{

	public static class Before {
		public int _id;
		
		public Before(int id) {
			_id = id;
		}
	}

	public static class After {
		
	}

	@Override
	protected void store() throws Exception {
		store(new Before(42));
	}

	public void testRetrieval() throws Exception {
		fixture().resetConfig();
		Configuration config = fixture().config();
		Reflector reflector = new ExcludingReflector(Before.class);
		config.reflectWith(reflector);
		TypeAlias alias = new TypeAlias(Before.class, After.class);
		config.addAlias(alias);
		defragment();
		
		After after = retrieveOnlyInstance(After.class);
		
		config = fixture().config();
		config.reflectWith(new ExcludingReflector());
		config.removeAlias(alias);
		reopen();
		
		Before before = retrieveOnlyInstance(Before.class);
		Assert.areEqual(42, before._id);
	}
}
