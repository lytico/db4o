package com.db4o.db4ounit.common.soda;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class PreserveJoinsTestCase extends AbstractDb4oTestCase {
	
	public static class Parent {
	
		public Parent(Child child, String value) {
			this.child = child;
			this.value = value;
		}

		public Child child;		
		public String value;
	}
	
	public static class Child {
		
		public Child(String name) {
			this.name = name;
		}

		public String name;
		
	}
	
	@Override
	protected void store() throws Exception {
		store(new Parent(new Child("bar"), "parent"));
	}
	
	public void test() {
		
		Query barQuery = db().query();
		barQuery.constrain(Child.class);
		barQuery.descend("name").constrain("bar");
		Object barObj = barQuery.execute().next();
		
		Query query = db().query();
		query.constrain(Parent.class);
		Constraint c1 = query.descend("value").constrain("dontexist");
		Constraint c2 = query.descend("child").constrain(barObj);
		Constraint c1_and_c2 = c1.and(c2);
		
		Constraint cParent = query.descend("value").constrain("parent");
		c1_and_c2.or(cParent);
		
		Assert.areEqual(1, query.execute().size());		
	}

}
