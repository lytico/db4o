package com.db4o.db4ounit.common.defragment;

import db4ounit.Assert;
import db4ounit.extensions.AbstractDb4oTestCase;

public class DefragmentUntypedPrimitiveArrayTestCase extends AbstractDb4oTestCase {

	private static final int ITEM_SIZE = 42;

	public static class Item {
		public int _id;
		public Object _intData;
		public Object _byteData;
		public String _name;
		
		public Item(int size) {
			_id = size;
			_intData = new int[size];
			_byteData = new byte[size];
			for(int idx = 0; idx < size; idx++) {
				((int[])_intData)[idx] = idx;
				((byte[])_byteData)[idx] = (byte)idx;
			}
			_name = String.valueOf(size);
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
		Assert.areEqual(ITEM_SIZE, item._id);
		Assert.areEqual(ITEM_SIZE, ((int[])item._intData).length);
		Assert.areEqual(ITEM_SIZE - 1, ((int[])item._intData)[ITEM_SIZE - 1]);
		Assert.areEqual(ITEM_SIZE, ((byte[])item._byteData).length);
		Assert.areEqual(ITEM_SIZE - 1, ((byte[])item._byteData)[ITEM_SIZE - 1]);
		Assert.areEqual(String.valueOf(ITEM_SIZE), item._name);
	}
}
