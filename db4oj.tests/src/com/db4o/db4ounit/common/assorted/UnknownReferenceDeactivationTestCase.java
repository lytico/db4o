package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;

public class UnknownReferenceDeactivationTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public String value;

		public Item(String value) {
	        this.value = value;
        }
		
	}
	
	public void test() {
		final Item item = new Item("my string");
		db().deactivate(item, Integer.MAX_VALUE);
		Assert.areEqual("my string", item.value);
	}

}
