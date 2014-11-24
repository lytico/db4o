/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class IndexOnParentFieldTestCase extends AbstractDb4oTestCase{
	
	public static class Parent {
		
		public Parent(String name) {
			_name = name;
		}

		public String _name;
		
	}
	
	public static class Child extends Parent {

		public Child(String name) {
			super(name);
		}
		
	}
	
	protected void store() {
		store(new Parent("one"));
		store(new Child("one"));
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Parent.class).objectField("_name").indexed(true);
	}
	
	public void test(){
		Query q = newQuery();
		q.constrain(Child.class);
		q.descend("_name").constrain("one");
		Assert.areEqual(1, q.execute().size());
	}

}
