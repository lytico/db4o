package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class NoTestConstructorsQEStringCmpTestCase extends AbstractDb4oTestCase implements OptOutAllButNetworkingCS {

	public static class Item {
		public String _name;
		
		public Item(String name) {
			_name = name;
		}
	}
	
	private interface ConstraintModifier {
		void modify(Constraint constraint);
	}
	
	protected void configure(Configuration config) throws Exception {
		config.callConstructors(true);
		config.testConstructors(false);
	}
	
	protected void store() throws Exception {
		store(new Item("abc"));
	}
	
	public void testStartsWith() {
		assertSingleItem("a", new ConstraintModifier() {
			public void modify(Constraint constraint) {
				constraint.startsWith(false);
			}
		});
	}

	public void testEndsWith() {
		assertSingleItem("c", new ConstraintModifier() {
			public void modify(Constraint constraint) {
				constraint.endsWith(false);
			}
		});
	}

	public void testContains() {
		assertSingleItem("b", new ConstraintModifier() {
			public void modify(Constraint constraint) {
				constraint.contains();
			}
		});
	}

	private void assertSingleItem(String pattern, ConstraintModifier modifier) {
		Query query = baseQuery(pattern, modifier);
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
	}
	
	private Query baseQuery(String pattern, ConstraintModifier modifier) {
		Query query = newQuery();
		query.constrain(Item.class);
		Constraint constraint = query.descend("_name").constrain(pattern);
		modifier.modify(constraint);
		return query;
	}

	public static void main(String[] args) {
		new NoTestConstructorsQEStringCmpTestCase().runNetworking();
	}
}
