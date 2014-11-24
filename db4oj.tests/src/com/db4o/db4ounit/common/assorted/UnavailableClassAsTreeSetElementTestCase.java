package com.db4o.db4ounit.common.assorted;

import java.util.Set;
import java.util.TreeSet;

import com.db4o.ObjectSet;

import db4ounit.Assert;
import db4ounit.extensions.OptOutExcludingClassLoaderIssue;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class UnavailableClassAsTreeSetElementTestCase extends UnavailableClassTestCaseBase implements OptOutExcludingClassLoaderIssue {
	
	public static class Item implements Comparable {
		private int _value;

		public Item(int value) {
			_value = value;
        }

		public int compareTo(Object o) {
			return _value - ((Item)o)._value;
        }
	}
	
	public static class Parent {
		Set<Item> _items = new TreeSet<Item>();
		
		public Parent(Item... items) {
			if (items != null)
			for (Item item : items) {
	            _items.add(item);
            }
		}
	}
	
	@Override
	protected void store() throws Exception {
	    store(new Parent(new Item(-1), new Item(42)));
	}
	
	public void testDefragment() throws Exception {
		reopenHidingClasses(Item.class);
		defragment();
	}
	
	public void testUnavailableItem() throws Exception {
		reopenHidingClasses(Item.class);
		
		final ObjectSet<Object> result = newQuery().execute();
		Assert.areEqual(4, result.size());
		for (Object object : result) {
	        Assert.isNotNull(object);
        }
	}

}
