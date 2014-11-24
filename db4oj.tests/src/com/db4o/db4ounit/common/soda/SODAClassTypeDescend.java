/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

// COR-471
public class SODAClassTypeDescend extends AbstractDb4oTestCase {

	public static class DataA {
		public DataB _val;
	}

	public static class DataB {
		public DataA _val;
	}

	public static class DataC {
		public DataC _next;
	}
	
	protected void store() throws Exception {
		DataA objectA = new DataA();
		DataB objectB = new DataB();
		objectA._val=objectB;
		objectB._val=objectA;
		store(objectB);
		// just to show that the descend to "_val" actually is
		// recognized - this one doesn't show up in the result
		store(new DataC());
	}
	
	public void testFieldConstrainedToType() {
		Query query = newQuery();
		query.descend("_val").constrain(DataA.class);
		ObjectSet result = query.execute();
		Assert.areEqual(1,result.size());
		Assert.isInstanceOf(DataB.class,result.next());
	}
}
