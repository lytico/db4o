/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeepSetTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new DeepSetTestCase().runConcurrency();
	}
	
	public DeepSetTestCase child;

	public String name;

	protected void store() {
		name = "1";
		child = new DeepSetTestCase();
		child.name = "2";
		child.child = new DeepSetTestCase();
		child.child.name = "3";
		store(this);
	}

	public void conc(ExtObjectContainer oc, int seq) {
		DeepSetTestCase example = new DeepSetTestCase();
		example.name = "1";
		DeepSetTestCase ds = (DeepSetTestCase) oc.queryByExample(example).next();
		Assert.areEqual("1", ds.name);
		Assert.areEqual("3", ds.child.child.name);
		ds.name = "1";
		ds.child.name = "12" + seq;
		ds.child.child.name = "13" + seq;
		oc.store(ds, 2);
	}

	public void check(ExtObjectContainer oc) {
		DeepSetTestCase example = new DeepSetTestCase();
		example.name = "1";
		DeepSetTestCase ds = (DeepSetTestCase) oc.queryByExample(example).next();
		Assert.isTrue(ds.child.name.startsWith("12"));
		Assert.isTrue(ds.child.name.length() > "12".length());
		Assert.areEqual("3", ds.child.child.name);
	}

}
