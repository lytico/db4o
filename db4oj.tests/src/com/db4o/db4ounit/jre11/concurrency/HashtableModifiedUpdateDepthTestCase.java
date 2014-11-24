/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.concurrency;

import java.util.*;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class HashtableModifiedUpdateDepthTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new HashtableModifiedUpdateDepthTestCase().runConcurrency();
	}
	
	public static class Item {
		public Hashtable ht;
	}

	protected void configure(Configuration config) {
		config.updateDepth(Integer.MAX_VALUE);
	}

	protected void store() {
		Item item = new Item();
		item.ht = new Hashtable();
		item.ht.put("hi", "five");
		store(item);
	}

	public void conc(ExtObjectContainer oc, int seq) {
		Hashtable ht = (Hashtable) retrieveOnlyInstance(oc, Hashtable.class);
		ht.put("hi", "updated" + seq);
		oc.store(ht);
	}

	public void check(ExtObjectContainer oc) {
		Hashtable ht = (Hashtable) retrieveOnlyInstance(oc, Hashtable.class);
		String s = (String) ht.get("hi");
		Assert.isTrue(s.startsWith("updated"));
		Assert.isTrue(s.length() > "updated".length());
	}
}