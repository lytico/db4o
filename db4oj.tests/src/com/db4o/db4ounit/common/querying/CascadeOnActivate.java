/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeOnActivate extends AbstractDb4oTestCase implements OptOutTA {

	public String name;

	public CascadeOnActivate child;

	protected void configure(Configuration conf) {
		conf.objectClass(this).cascadeOnActivate(true);
	}

	protected void store() {
		CascadeOnActivate coa = new CascadeOnActivate();
		coa.name = "1";
		coa.child = new CascadeOnActivate();
		coa.child.name = "2";
		coa.child.child = new CascadeOnActivate();
		coa.child.child.name = "3";

		db().store(coa);
	}

	public void test() {
		Query q = newQuery(getClass());
		q.descend("name").constrain("1");
		ObjectSet os = q.execute();

		CascadeOnActivate coa = (CascadeOnActivate) os.next();
		CascadeOnActivate coa3 = coa.child.child;

		Assert.areEqual("3", coa3.name);

		db().deactivate(coa, Integer.MAX_VALUE);

		Assert.isNull(coa3.name);

		db().activate(coa, 1);

		Assert.areEqual("3", coa3.name);
	}
}
