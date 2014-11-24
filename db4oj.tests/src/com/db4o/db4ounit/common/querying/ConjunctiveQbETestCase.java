/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ConjunctiveQbETestCase extends AbstractDb4oTestCase {

	public static class Sup {
		public boolean _flag;

		public Sup(boolean flag) {
			this._flag = flag;
		}
		
		public ObjectSet query(ObjectContainer db) {
			Query query=db.query();
			query.constrain(this);
			query.descend("_flag").constrain(Boolean.TRUE).not();
			return query.execute();
		}
	}

	public static class Sub1 extends Sup {
		public Sub1(boolean flag) {
			super(flag);
		}
	}

	public static class Sub2 extends Sup {
		public Sub2(boolean flag) {
			super(flag);
		}
	}
	
	protected void store() throws Exception {
		store(new Sub1(false));
		store(new Sub1(true));
		store(new Sub2(false));
		store(new Sub2(true));
	}
	
	public void testAndedQbE() {
		Assert.areEqual(1,new Sub1(false).query(db()).size());
	}
}
