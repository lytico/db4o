/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.ordered;

import com.db4o.*;
import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.query.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Remove(decaf.Platform.JDK11)
public class OrderByWithComparableTestCase extends AbstractDb4oTestCase {

	public static class ItemComparable implements Comparable, Activatable {

		private transient Activator _activator;
		
		public int _id;
		
		public ItemComparable(int id) {
			_id = id;
		}
		
		public int compareTo(Object other) {
			activate(ActivationPurpose.READ);
			
			ItemComparable cmp = (ItemComparable) other;
			if(_id == cmp.id()) {
				return 0;
			}
			return _id < cmp.id() ? -1 : 1;
		}
		
		public int id() {
			activate(ActivationPurpose.READ);
			return _id;
		}
		
		@Override
		public String toString() {
			activate(ActivationPurpose.READ);
			return "ItemComparable(" + _id + ")";
		}
		
		public void bind(Activator activator) {
			if (_activator == activator) return;
			
			if (_activator != null && activator != null) {
				throw new RuntimeException();
			}
			
			_activator = activator;
		}

		public void activate(ActivationPurpose purpose) {
			if (_activator != null) {
				_activator.activate(purpose);
			}
		}		
	}
	
	public static class Item {
		public int _id;
		public ItemComparable _itemCmp;
		
		public Item(int id, ItemComparable itemCmp) {
			_id = id;
			_itemCmp = itemCmp;
		}
		
		public ItemComparable itemCmp() {
			return _itemCmp;
		}
		
		@Override
		public String toString() {
			return "Item(" + _itemCmp + ")"; 
		}
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentActivationSupport());
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(3, new ItemComparable(2)));
		store(new Item(2, null));
		store(new Item(1, new ItemComparable(1)));
		store(new Item(4, null));
	}

	public void testOrderByWithEnums() {
		Query query = newQuery();
		query.constrain(Item.class);
		query.descend("_id").constrain(1).or(query.descend("_id").constrain(3));
		query.descend("_itemCmp").orderAscending();
		ObjectSet<Item> result = query.execute();
		Assert.areEqual(2, result.size());		
		Assert.areEqual(1, result.next().itemCmp().id());
		Assert.areEqual(2, result.next().itemCmp().id());
	}

	public void testOrderByWithNullValues() {
		Query query = newQuery();
		query.constrain(Item.class);
		query.descend("_itemCmp").orderAscending();
		ObjectSet<Item> result = query.execute();
		Assert.areEqual(4, result.size());
		Assert.isNull(result.next().itemCmp());
		Assert.isNull(result.next().itemCmp());
		Assert.areEqual(1, result.next().itemCmp().id());
		Assert.areEqual(2, result.next().itemCmp().id());
	}
	
}
