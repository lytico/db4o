/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public abstract class StringEncodingTestCaseBase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public Item(String name){
			_name = name;
		}
		
		public String _name;
	}
	
	public void testStoreSimpleObject() throws Exception{
		String name = "one";
		store(new Item(name));
		reopen();
		Item item = (Item) retrieveOnlyInstance(Item.class);
		Assert.areEqual(name, item._name);
	}
	
	public void testCorrectStringIoClass(){
		Assert.areSame(stringIoClass(), container().stringIO().getClass());
	}
	
	protected abstract Class stringIoClass();


}
