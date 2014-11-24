/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import db4ounit.*;

public class ServerObjectContainerIsolationTestCase extends EmbeddedAndNetworkingClientTestCaseBase {
	
	public static class Item {
		
		public Item(String name){
			_name = name;
		}
		
		public String _name;
		
	}
	
	public void testStoringNewItem(){
		serverObjectContainer().store(new Item("original"));
		Assert.areEqual(0, networkingClient().query(Item.class).size());
		Assert.areEqual(1, serverObjectContainer().query(Item.class).size());
	}
	

}
