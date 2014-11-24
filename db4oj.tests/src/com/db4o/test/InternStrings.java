/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class InternStrings {
	public String _name;
	
	public InternStrings() {
		this(null);
	}

	public InternStrings(String name) {
		_name = name;
	}

	public void configure() {
		Db4o.configure().internStrings(true);
	}
	
	public void store() {
        Test.deleteAllInstances(this);
		String name="Foo";
		Test.store(new InternStrings(name));
		Test.store(new InternStrings(name));
	}
	
	public void test() {
		Query query=Test.query();
		query.constrain(getClass());
		ObjectSet result=query.execute();
		Test.ensureEquals(2, result.size());
		InternStrings first=(InternStrings)result.next();
		InternStrings second=(InternStrings)result.next();
		Test.ensure(first._name==second._name);
	}
}
