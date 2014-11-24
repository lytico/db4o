/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.persistent.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class IndexedByIdentityTestCase extends Db4oClientServerTestCase {
	public static void main(String[] args) {
		new IndexedByIdentityTestCase().runConcurrency();
	}

	public Atom atom;

	static final int COUNT = 10;

	protected void configure(Configuration config) {
		config.objectClass(this).objectField("atom").indexed(true);
	    config.objectClass(IndexedByIdentityTestCase.class).cascadeOnUpdate(true);

	}

	protected void store() {
		for (int i = 0; i < COUNT; i++) {
			IndexedByIdentityTestCase ibi = new IndexedByIdentityTestCase();
			ibi.atom = new Atom("ibi" + i);
			store(ibi);
		}
	}

	public void concRead(ExtObjectContainer oc) {
		for (int i = 0; i < COUNT; i++) {
			Query q = oc.query();
			q.constrain(Atom.class);
			q.descend("name").constrain("ibi" + i);
			ObjectSet objectSet = q.execute();
			Assert.areEqual(1, objectSet.size());
			Atom child = (Atom) objectSet.next();
			q = oc.query();
			q.constrain(IndexedByIdentityTestCase.class);
			q.descend("atom").constrain(child).identity();
			objectSet = q.execute();
			Assert.areEqual(1, objectSet.size());
			IndexedByIdentityTestCase ibi = (IndexedByIdentityTestCase) objectSet.next();
			Assert.areSame(child, ibi.atom);
		}

	}

	public void concUpdate(ExtObjectContainer oc, int seq) throws Exception {
		Query q = oc.query();
		q.constrain(IndexedByIdentityTestCase.class);
		ObjectSet os = q.execute();
		Assert.areEqual(COUNT, os.size());
		while (os.hasNext()) {
			IndexedByIdentityTestCase idi = (IndexedByIdentityTestCase) os.next();
			idi.atom.name = "updated" + seq;
			oc.store(idi);
			Thread.sleep(100);
		}
	}

	public void checkUpdate(ExtObjectContainer oc) {
		Query q = oc.query();
		q.constrain(IndexedByIdentityTestCase.class);
		ObjectSet os = q.execute();
		Assert.areEqual(COUNT, os.size());
		String expected = null;
		while (os.hasNext()) {
			IndexedByIdentityTestCase idi = (IndexedByIdentityTestCase) os.next();
			if (expected == null) {
				expected = idi.atom.name;
				Assert.isTrue(expected.startsWith("updated"));
				Assert.isTrue(expected.length() > "updated".length());
			}
			Assert.areEqual(expected, idi.atom.name);
		}
	}

}
