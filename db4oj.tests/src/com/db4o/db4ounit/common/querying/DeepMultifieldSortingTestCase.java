/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeepMultifieldSortingTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public int _id;
		
		public ItemChild _typedChild;
		
		public Object _untypedChild;
		
	    public Item(int id, ItemChild typedChild, ItemChild untypedChild) {
	    	_id = id;
	    	_typedChild = typedChild;
	    	_untypedChild = untypedChild;
		}
	} 

	public static class ItemChild {
		
		public int _id;
		
	    public ItemChild(int id) {
	    	_id = id;
		}
	} 
	
	@Override
	protected void store() throws Exception {
		storeItems(1,2,3);
		storeItems(3,2,3);
		storeItems(2,2,2);
		storeItems(2,1,1);
		storeItems(2,3,3);
	}

	private void storeItems(int parentId, int typedChildId, int untypedChildId) {
		store(new Item(parentId, new ItemChild(typedChildId), new ItemChild(untypedChildId)));
	}
	
	public void testTypedChild(){
		assertOrdering("_typedChild");
	}
	
	/**
	 * #COR-1771 Sorting by untyped fields is not supported.
	 */
	public void _testUntypedChild(){
		assertOrdering("_untypedChild");
	}

	private void assertOrdering(String childFieldName) {
		Query query = db().query(); 
		query.constrain(Item.class); 
		query.descend("_id").orderAscending(); 
		query.descend(childFieldName).descend("_id").orderAscending(); 
		ObjectSet<Item> objectSet = query.execute();
		Assert.areEqual(5, objectSet.size());
		Item lastItem = new Item(0, new ItemChild(0), null);
		while(objectSet.hasNext()){
			Item item = objectSet.next();
			Assert.isGreaterOrEqual(lastItem._id, item._id);
			if(item._id == lastItem._id){
				Assert.isGreaterOrEqual(lastItem._typedChild._id, item._typedChild._id);
			}
			lastItem = item;
		}
	}

}
