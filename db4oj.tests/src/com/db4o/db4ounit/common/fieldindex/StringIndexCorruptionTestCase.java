/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;

/**
 * Jira ticket: COR-373
 * 
 * @exclude
 */
public class StringIndexCorruptionTestCase extends StringIndexTestCaseBase {
	
	public static void main(String[] arguments) {
		new StringIndexCorruptionTestCase().runSolo();
	}
	
	protected void configure(Configuration config) {
		super.configure(config);
		config.bTreeNodeSize(4);
	}
	
	public void testStressSet() {		
    	final ExtObjectContainer container = db();
    	
    	final int itemCount = 300;
		for (int i=0; i<itemCount; ++i) {
    		Item item = new Item(itemName(i));
    		container.store(item);
    		container.store(item);
    		container.commit();
    		container.store(item);
    		container.store(item);
    		container.commit();
    	}    	
    	for (int i=0; i<itemCount; ++i) {
    		String itemName = itemName(i);
    		final Item found = query(itemName);
    		Assert.isNotNull(found, "'" + itemName + "' not found");
			Assert.areEqual(itemName, found.name);
    	}
    }
	
	private String itemName(int i) {
		return "item " + i;
	}

}
