/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class InternStringsTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new InternStringsTestCase().runConcurrency();
	}

	public String _name;

	public InternStringsTestCase() {
		this(null);
	}

	public InternStringsTestCase(String name) {
		_name = name;
	}

	protected void configure(Configuration config) {
		config.internStrings(true);
	}

	protected void store() {
		String name = "Foo";
		store(new InternStringsTestCase(name));
		store(new InternStringsTestCase(name));
	}

	public void conc(ExtObjectContainer oc) {
		Query query = oc.query();
		query.constrain(InternStringsTestCase.class);
		ObjectSet result = query.execute();
		Assert.areEqual(2, result.size());
		InternStringsTestCase first = (InternStringsTestCase) result.next();
		InternStringsTestCase second = (InternStringsTestCase) result.next();
		Assert.areSame(first._name, second._name);
	}
}
