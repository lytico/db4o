/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.concurrency;

import java.util.*;

import com.db4o.config.*;
import com.db4o.db4ounit.common.persistent.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeToExistingVectorMemberTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new CascadeToExistingVectorMemberTestCase().runConcurrency();
	}
	
	public static class Item {
		public Vector vec;
	}

	protected void configure(Configuration config) {
		config.objectClass(Item.class).cascadeOnUpdate(true);
		config.objectClass(Atom.class).cascadeOnUpdate(false);
	}

	protected void store() {
		Item item = new Item();
		item.vec = new Vector();
		Atom atom = new Atom("one");
		store(atom);
		item.vec.addElement(atom);
		store(item);
	}

	public void conc(final ExtObjectContainer oc, final int seq) {
		Item item = (Item) retrieveOnlyInstance(oc, Item.class);
		Atom atom = (Atom) item.vec.elementAt(0);
		atom.name = "two" + seq;
		oc.store(item);
		atom.name = "three" + seq;
		oc.store(item);
	}

	public void check(final ExtObjectContainer oc) {
		Item item = (Item) retrieveOnlyInstance(oc, Item.class);
		Atom atom = (Atom) item.vec.elementAt(0);
		Assert.isTrue(atom.name.startsWith("three"));
		Assert.isTrue(atom.name.length() > "three".length());	
	}
}
