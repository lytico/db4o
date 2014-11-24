package com.db4o.db4ounit.jre12.defragment;

import java.util.ArrayList;
import java.util.List;

import db4ounit.Assert;
import db4ounit.extensions.AbstractDb4oTestCase;

@decaf.Remove(decaf.Platform.JDK11)
public class DefragmentPrimitiveArrayInCollectionTestCase extends AbstractDb4oTestCase {

	private static final int ITEM_SIZE = 42;

	public static class Item {
		public List _data;
		
		public Item(int size) {
			_data = new ArrayList();
			_data.add(new byte[size]);
		}
	}

	protected void store() throws Exception {
		store(new Item(ITEM_SIZE));
	}
	
	public void testDefragment() throws Exception {
		assertItemSizes();
		defragment();
		assertItemSizes();
	}

	private void assertItemSizes() {
		Item item = (Item) retrieveOnlyInstance(Item.class);
		Assert.areEqual(1, item._data.size());
		Assert.areEqual(ITEM_SIZE, ((byte[])item._data.get(0)).length);
	}
}
