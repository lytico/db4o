/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class GreaterOrEqualTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new GreaterOrEqualTestCase().runConcurrency();
	}

	public int val;

	public GreaterOrEqualTestCase() {

	}

	public GreaterOrEqualTestCase(int val) {
		this.val = val;
	}

	protected void store() {
		store(new GreaterOrEqualTestCase(1));
		store(new GreaterOrEqualTestCase(2));
		store(new GreaterOrEqualTestCase(3));
		store(new GreaterOrEqualTestCase(4));
		store(new GreaterOrEqualTestCase(5));
	}

	public void conc(ExtObjectContainer oc) {
		int[] expect = { 3, 4, 5 };
		Query q = oc.query();
		q.constrain(GreaterOrEqualTestCase.class);
		q.descend("val").constrain(new Integer(3)).greater().equal();
		ObjectSet res = q.execute();
		while (res.hasNext()) {
			GreaterOrEqualTestCase r = (GreaterOrEqualTestCase) res.next();
			for (int i = 0; i < expect.length; i++) {
				if (expect[i] == r.val) {
					expect[i] = 0;
				}
			}
		}
		for (int i = 0; i < expect.length; i++) {
			Assert.areEqual(0, expect[i]);
		}
	}

}
