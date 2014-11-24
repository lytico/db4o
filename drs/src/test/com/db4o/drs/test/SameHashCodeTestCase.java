/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.drs.test;


public class SameHashCodeTestCase extends EqualsHashCodeOverriddenTestCaseBase {

	public static class Holder {
		Item _itemA;
		Item _itemB;
		
		public Holder(Item itemA, Item itemB) {
			_itemA = itemA;
			_itemB = itemB;
		}
	}
	
	public void testReplicatesSimpleHolder() {
		assertReplicates(new Holder(new Item("item"), new Item("item")));
	}
}
