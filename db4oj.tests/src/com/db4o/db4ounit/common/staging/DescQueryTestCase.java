/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.staging;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

// COR-2141
public class DescQueryTestCase extends AbstractDb4oTestCase {

	public static class Item {
		public int _id;
		public Item _child;

		public Item(int id) {
			this(id, null);
		}

		public Item(int id, Item child) {
			this._id = id;
			this._child = child;
		}
		
		@Override
		public boolean equals(Object other) {
			if(other == this) {
				return true;
			}
			if(other == null || getClass() != other.getClass()) {
				return false;
			}
			Item item = (Item)other;
			if(_id != item._id) {
				return false;
			}
			return _child == null ? item._child == null : _child.equals(item._child);
		}
		
		@Override
		public int hashCode() {
			return _id;
		}
		
		@Override
		public String toString() {
			return _id + "[" + (_child == null ? "" : _child.toString()) + "]";
		}
	}

	@Override
	protected void store() throws Exception {
		store(new Item(42, new Item(43)));
		store(new Item(42, new Item(44)));
		store(new Item(45, new Item(46)));
	}
	
	public void testDescQuery() {
		Query query = newQuery(Item.class);
		Query sq = query.descend("_child");
		query.descend("_id").constrain(42).and(sq.descend("_id").constrain(43).greater());
		ObjectSet<Item> result = sq.<Item>execute();
		Iterator4Assert.sameContent(Iterators.iterate(new Item(44)), Iterators.iterator(result));
	}
}
