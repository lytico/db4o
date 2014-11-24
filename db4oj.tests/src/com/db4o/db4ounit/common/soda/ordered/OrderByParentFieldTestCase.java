/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda.ordered;

import com.db4o.query.*;

import db4ounit.extensions.*;

/**
 * @exclude
 */
public class OrderByParentFieldTestCase extends AbstractDb4oTestCase {
	
	public static class Parent {
		
		public String _name;

		public Parent(String name) {
			_name = name;
		}
		
	}
	
	public static class Child extends Parent {
		
		public int _age;
		
		public Child(String name, int age) {
			super(name);
			_age = age;
		}
		
	}
	
	@Override
	protected void store() throws Exception {
		store(new Child("One", 1));
		store(new Child("Two", 2));
	}
	
	public void test() throws Exception{
		Query query = newQuery(Child.class);
		query.descend("_name").orderAscending();
		query.execute();
	}

}
