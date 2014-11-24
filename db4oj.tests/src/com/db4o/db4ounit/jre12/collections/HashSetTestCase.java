package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import db4ounit.extensions.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class HashSetTestCase extends AbstractDb4oTestCase {
	
	public static class Item {

		private LinkedHashSet<SubItem> _items;

		public Item(SubItem...items) {
			_items = new LinkedHashSet<SubItem>(Arrays.asList(items));
        }
		
		public int size() {
			return _items.size();
		}
		
	}
	
	public static class SubItem {

		private String _name;

		public SubItem(String name) {
			_name = name;
        }
		
		@Override
		public int hashCode() {
			return _name.hashCode();
		}
	}
	
	@Override
	protected void store() throws Exception {
	    store(new Item(new SubItem("foo"), new SubItem("bar")));
	}
	
	public void testPeekPersisted() {
		
//		HardObjectReference.peekPersisted(trans(), db().getID(retrieveOnlyInstance(Item.class)));
	}

}
