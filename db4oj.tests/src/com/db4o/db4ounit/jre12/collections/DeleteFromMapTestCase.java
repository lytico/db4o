/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeleteFromMapTestCase extends AbstractDb4oTestCase{

	private final static String KEY_ID = "key";
	private final static String VALUE_ID = "value";
	
	public static class Holder {
		public Map<Item, Item> _map = new HashMap<Item, Item>();
	}
	
	public static class Item {
		public String _id;
		
		public Item(String id) {
			_id = id;
		}
	}
	
	@Override
	protected void store() throws Exception {
		Holder holder = new Holder();
		holder._map.put(new Item(KEY_ID), new Item(VALUE_ID));
		store(holder);
	}
	
	public void testDeleteKey(){
		Item keyItem = storedItem(KEY_ID);
		db().delete(keyItem);
		db().commit();
		Holder holder = retrieveOnlyInstance(Holder.class);
		Assert.areEqual(0, holder._map.size());
	}

	public void testDeleteValue(){
		Item valueItem = storedItem(VALUE_ID);
		db().delete(valueItem);
		db().commit();
		Holder holder = retrieveOnlyInstance(Holder.class);
		Assert.areEqual(1, holder._map.size());
		Assert.isNull(holder._map.get(storedItem(KEY_ID)));
	}

	private Item storedItem(String id) {
		Query query = newQuery(Item.class);
		query.descend("_id").constrain(id);
		ObjectSet<Item> result = query.execute();
		Assert.isTrue(result.hasNext());
		return result.next();
	}
}
