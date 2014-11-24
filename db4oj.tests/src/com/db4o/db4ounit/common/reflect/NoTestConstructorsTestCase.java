package com.db4o.db4ounit.common.reflect;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class NoTestConstructorsTestCase extends AbstractDb4oTestCase {

	public static class Item {
		public static int constructorCalls = 0;
		
		public Item() {
			constructorCalls++;
		}
	}

	protected void db4oSetupBeforeStore() {
		Item.constructorCalls = 0;
	}
	
	protected void configure(Configuration config) {
		config.callConstructors(true);
		config.testConstructors(false);
	}
	
	protected void store() {
		store(new Item());
		Assert.areEqual(1, Item.constructorCalls);
	}

	public void test() {
		retrieveOnlyInstance(Item.class);
		Assert.areEqual(2, Item.constructorCalls);
	}
	
	public static void main(String[] args) {
		new NoTestConstructorsTestCase().runAll();
	}
}
