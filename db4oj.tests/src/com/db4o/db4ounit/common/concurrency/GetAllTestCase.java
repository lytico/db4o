/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class GetAllTestCase extends Db4oClientServerTestCase {
	public static void main(String[] args) {
		new GetAllTestCase().runConcurrency();
	}

	protected void store() {
		store(new GetAllTestCase());
		store(new GetAllTestCase());
	}

	public void conc(ExtObjectContainer oc) {
		Assert.areEqual(2, oc.queryByExample(null).size());
	}

	public void concSODA(ExtObjectContainer oc) {
		Assert.areEqual(2, oc.query().execute().size());
	}
}
