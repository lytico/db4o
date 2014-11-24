/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class QueryForUnknownFieldTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] args) {
		new QueryForUnknownFieldTestCase().runConcurrency();
	}

	public String _name;

	public QueryForUnknownFieldTestCase() {
	}

	public QueryForUnknownFieldTestCase(String name) {
		_name = name;
	}

	protected void store() {
		_name = "name";
		store(this);
	}

	public void conc(ExtObjectContainer oc) {
		Query q = oc.query();
		q.constrain(QueryForUnknownFieldTestCase.class);
		q.descend("_name").constrain("name");
		Assert.areEqual(1, q.execute().size());

		q = oc.query();
		q.constrain(QueryForUnknownFieldTestCase.class);
		q.descend("name").constrain("name");
		Assert.areEqual(0, q.execute().size());
	}

}
