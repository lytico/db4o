/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.store;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeepSetTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new DeepSetTestCase().runSolo();
	}
	
	public static class Item {
		public Item child;
		public String name;
	}

	private Item _item;
	
	protected void store() {
		_item = new Item();
		_item.name = "1";
		_item.child = new Item();
		_item.child.name = "2";
		_item.child.child = new Item();
		_item.child.child.name = "3";
		store(_item);
	}

	public void test() throws Exception {
        ExtObjectContainer oc = db(); 
        _item.name = "1";
        Item item = (Item)oc.queryByExample(_item).next();
        item.name="11";
        item.child.name = "12";
        oc.store(item, 2);
        oc.deactivate(item, Integer.MAX_VALUE);
        item.name = "11";
        item = (Item)oc.queryByExample(item).next();
        Assert.areEqual("12", item.child.name);
        Assert.areEqual("3", item.child.child.name);
    }

}
