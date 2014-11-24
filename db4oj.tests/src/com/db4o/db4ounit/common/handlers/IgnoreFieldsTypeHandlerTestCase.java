/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.config.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class IgnoreFieldsTypeHandlerTestCase extends AbstractDb4oTestCase {
	
	public static class Item1 {
		
		public int id1;
		
	}
	
	public static class Item2 extends Item1 {
		
		public int id2;
		
	}

	public static class Item3 extends Item2 {
		
		public int id3;
		
	}
	
	public static class Item4 extends Item3 {
		
		public int id4;
		
	}
	
	public static class Item5 extends Item4 {
		
		public int id5;
		
	}
	
	protected void configure(Configuration config) throws Exception {
		config.registerTypeHandler(new SingleClassTypeHandlerPredicate(Item2.class), IgnoreFieldsTypeHandler.INSTANCE);
		config.registerTypeHandler(new SingleClassTypeHandlerPredicate(Item4.class), IgnoreFieldsTypeHandler.INSTANCE);
	}
	
	protected void store() throws Exception {
		Item5 item = new Item5();
		item.id1 = 1;
		item.id2 = 2;
		item.id3 = 3;
		item.id4 = 4;
		item.id5 = 5;
		store(item);
	}
	
	public void test(){
		Item5 item = (Item5) retrieveOnlyInstance(Item5.class);
		Assert.areEqual(1, item.id1);
		Assert.areEqual(0, item.id2);
		Assert.areEqual(3, item.id3);
		Assert.areEqual(0, item.id4);
		Assert.areEqual(5, item.id5);
	}

}
