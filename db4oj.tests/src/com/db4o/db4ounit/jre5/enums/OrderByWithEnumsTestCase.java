/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre5.enums;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class OrderByWithEnumsTestCase extends AbstractDb4oTestCase {

	public static enum ItemEnum {
		FIRST, SECOND
	}
	
	public static class Item {
		public int _id;
		public ItemEnum _itemEnum;
		
		public Item(int id, ItemEnum itemEnum) {
			_id = id;
			_itemEnum = itemEnum;
		}
		
		public ItemEnum itemEnum() {
			return _itemEnum;
		}
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(1, ItemEnum.FIRST));
		store(new Item(2, null));
		store(new Item(3, ItemEnum.SECOND));
		store(new Item(4, null));
	}

	public void testOrderByWithEnums() {
		Query query = newQuery();
		query.constrain(Item.class);
		query.descend("_id").constrain(1).or(query.descend("_id").constrain(3));
		query.descend("_itemEnum").orderAscending();
		ObjectSet<Item> result = query.execute();
		Assert.areEqual(2, result.size());
		Assert.areEqual(ItemEnum.FIRST, result.next().itemEnum());
		Assert.areEqual(ItemEnum.SECOND, result.next().itemEnum());
	}

	public void testOrderByWithNullValues() {
		Query query = newQuery();
		query.constrain(Item.class);
		query.descend("_itemEnum").orderAscending();
		ObjectSet<Item> result = query.execute();
		Assert.areEqual(4, result.size());
		Assert.isNull(result.next().itemEnum());
		Assert.isNull(result.next().itemEnum());
		Assert.areEqual(ItemEnum.FIRST, result.next().itemEnum());
		Assert.areEqual(ItemEnum.SECOND, result.next().itemEnum());
	}
	
}
