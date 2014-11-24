/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.concurrency;

import java.util.*;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class Circular2TestCase extends Db4oClientServerTestCase {

	public Hashtable ht;

	public String name;

	protected void configure(Configuration config) {
		config.updateDepth(Integer.MAX_VALUE);
	}

	protected void store() {
		ht = new Hashtable();
		name = "parent";
		C2C c2c = new C2C();
		c2c.parent = this;
		ht.put("test", c2c);
		store(ht);
	}

	public void conc(ExtObjectContainer oc) {
		ht = (Hashtable) retrieveOnlyInstance(oc, Hashtable.class);
		C2C c2c = (C2C) ht.get("test");
		Assert.areEqual("parent", c2c.parent.name);
	}

	public static class C2C {
		public Circular2TestCase parent;
	}
}
