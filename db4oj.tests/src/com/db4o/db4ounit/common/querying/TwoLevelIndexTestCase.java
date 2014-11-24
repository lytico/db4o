/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TwoLevelIndexTestCase extends AbstractDb4oTestCase {
	
	public static class Parent1 {
		public Child1 child;
	}
	
	public static class Parent2 extends Parent1 {
		
	}
	
	
	public static class Child1 {
		public int id;
	}
	
	public static class Child2 extends Child1 {
		
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Parent1.class).objectField("child").indexed(true);
		config.objectClass(Child1.class).objectField("id").indexed(true);
	}
	
	@Override
	protected void store() throws Exception {
		Parent1 parent1 = new Parent1();
		parent1.child = new Child1();
		parent1.child.id = 42;
		store(parent1);
		
		Parent2 parent2 = new Parent2();
		parent2.child = new Child2();
		parent2.child.id = 42;
		store(parent2);
	}
	
	public void testTwoLevelParentIsSubclassed(){
		Query query = db().query();
		query.constrain(Parent2.class);
		query.descend("child").descend("id").constrain(42);
		Assert.areEqual(1, query.execute().size());
	}
	
	public void testChildClassConstrained(){
		Query query = db().query();
		query.constrain(Parent1.class);
		query.descend("child").descend("id").constrain(42);
		query.descend("child").constrain(Child2.class);
		Assert.areEqual(1, query.execute().size());
	}

}
