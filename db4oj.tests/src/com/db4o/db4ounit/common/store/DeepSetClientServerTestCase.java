/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.store;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeepSetClientServerTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] args) {
		new DeepSetClientServerTestCase().runAll();
	}
	
	public static class Item {
		public Item child;
		public String name;
	}

	protected void store() {
		Item item = new Item();
		item.name = "1";
		item.child = new Item();
		item.child.name = "2";
		item.child.child = new Item();
		item.child.child.name = "3";
		store(item);
	}
	
	public void test() throws Exception {
		ExtObjectContainer oc1 = openNewSession();
		ExtObjectContainer oc2 = openNewSession();
		ExtObjectContainer oc3 = openNewSession();
		Item example = new Item();
		example.name = "1";
		try {
			Item item1 = (Item) oc1.queryByExample(example).next();
			Assert.areEqual("1", item1.name);
			Assert.areEqual("2", item1.child.name);
			Assert.areEqual("3", item1.child.child.name);

			Item item2 = (Item) oc2.queryByExample(example).next();
			Assert.areEqual("1", item2.name);
			Assert.areEqual("2", item2.child.name);
			Assert.areEqual("3", item2.child.child.name);

			item1.child.name = "12";
			item1.child.child.name = "13";
			oc1.store(item1, 2);
			oc1.commit();

			// check result
			Item item = (Item) oc1.queryByExample(example).next();
			Assert.areEqual("1", item.name);
			Assert.areEqual("12", item.child.name);
			Assert.areEqual("13", item.child.child.name);

			item = (Item) oc2.queryByExample(example).next();
			oc2.refresh(item, 3);
			Assert.areEqual("1", item.name);
			Assert.areEqual("12", item.child.name);
			Assert.areEqual("3", item.child.child.name);

			item = (Item) oc3.queryByExample(example).next();
			Assert.areEqual("1", item.name);
			Assert.areEqual("12", item.child.name);
			Assert.areEqual("3", item.child.child.name);
		} finally {
			oc1.close();
			oc2.close();
			oc3.close();
		}
	}

}
