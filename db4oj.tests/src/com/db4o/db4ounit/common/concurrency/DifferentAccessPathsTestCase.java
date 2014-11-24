/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DifferentAccessPathsTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new DifferentAccessPathsTestCase().runConcurrency();
	}
	
	public String foo;

	protected void store() {
		DifferentAccessPathsTestCase dap = new DifferentAccessPathsTestCase();
		dap.foo = "hi";
		store(dap);
		dap = new DifferentAccessPathsTestCase();
		dap.foo = "hi too";
		store(dap);
	}

	public void conc(ExtObjectContainer oc) throws Exception {
		DifferentAccessPathsTestCase dap = query(oc);
		for (int i = 0; i < 10; i++) {
			Assert.areSame(dap, query(oc));
		}
		oc.purge(dap);
		Assert.areNotSame(dap, query(oc));
	}

	private DifferentAccessPathsTestCase query(ExtObjectContainer oc) {
		Query q = oc.query();
		q.constrain(DifferentAccessPathsTestCase.class);
		q.descend("foo").constrain("hi");
		ObjectSet os = q.execute();
		Assert.areEqual(1, os.size());
		DifferentAccessPathsTestCase dap = (DifferentAccessPathsTestCase) os.next();
		return dap;
	}

}
