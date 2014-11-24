/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class RefreshTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new RefreshTestCase().runConcurrency();
	}

	public String name;

	public RefreshTestCase child;

	public RefreshTestCase() {

	}

	public RefreshTestCase(String name, RefreshTestCase child) {
		this.name = name;
		this.child = child;
	}

	protected void store() {
		RefreshTestCase r3 = new RefreshTestCase("o3", null);
		RefreshTestCase r2 = new RefreshTestCase("o2", r3);
		RefreshTestCase r1 = new RefreshTestCase("o1", r2);
		store(r1);
	}

	public void conc(ExtObjectContainer oc) {
		RefreshTestCase r11 = getRoot(oc);
		r11.name = "cc";
		oc.refresh(r11, 0);
		Assert.areEqual("cc", r11.name);
		oc.refresh(r11, 1);
		Assert.areEqual("o1", r11.name);
		r11.child.name = "cc";
		oc.refresh(r11, 1);
		Assert.areEqual("cc", r11.child.name);
		oc.refresh(r11, 2);
		Assert.areEqual("o2", r11.child.name);
	}

	private RefreshTestCase getRoot(ObjectContainer oc) {
		return getByName(oc, "o1");
	}

	private RefreshTestCase getByName(ObjectContainer oc, final String name) {
		Query q = oc.query();
		q.constrain(RefreshTestCase.class);
		q.descend("name").constrain(name);
		ObjectSet objectSet = q.execute();
		return (RefreshTestCase) objectSet.next();
	}

}
