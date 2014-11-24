/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeOnActivateTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new CascadeOnActivateTestCase().runConcurrency();
	}
	
	public static class Item {
		public String name;

		public Item child;
	}

	protected void configure(Configuration config) {
		config.objectClass(Item.class).cascadeOnActivate(true);
	}

	protected void store() {
		Item item = new Item();
		item.name = "1";
		item.child = new Item();
		item.child.name = "2";
		item.child.child = new Item();
		item.child.child.name = "3";
		store(item);
	}

	public void conc(ExtObjectContainer oc) {
		Query q = oc.query();
		q.constrain(Item.class);
		q.descend("name").constrain("1");
		ObjectSet os = q.execute();
		Item item = (Item) os.next();
		Item item3 = item.child.child;
		Assert.areEqual("3", item3.name);
		oc.deactivate(item, Integer.MAX_VALUE);
		Assert.isNull(item3.name);
		oc.activate(item, 1);
		Assert.areEqual("3", item3.name);
	}
}
