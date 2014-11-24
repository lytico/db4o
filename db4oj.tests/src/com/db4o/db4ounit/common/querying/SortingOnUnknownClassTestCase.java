/* Copyright (C) 2004 - 2012  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class SortingOnUnknownClassTestCase extends AbstractDb4oTestCase{
	
	public static class UnknownClass {
		public String name;
	}
	
	public void test() {
		Query query = newQuery(UnknownClass.class);
		query.descend("name").orderAscending();
		ObjectSet<Object> result = query.execute();
		Assert.areEqual(0, result.size());
	}

}
