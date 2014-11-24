/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.staging;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * #COR-1790
 */
public class UntypedFieldSortingTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public Item(Object id) {
			_id = id;
		}

		public Object _id;
		
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(2));
		store(new Item(3));
		store(new Item(1));
	}
	
	public void test(){
		Query query = db().query(); 
		query.constrain(Item.class); 
		query.descend("_id").orderAscending(); 
		ObjectSet<Item> objectSet = query.execute();
		int lastId = 0;
		while(objectSet.hasNext()){
			Item item = objectSet.next();
			int currentId = ((Integer)item._id).intValue();
			Assert.isGreater(lastId, currentId);
			currentId = lastId;
		}
	}

}
