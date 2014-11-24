/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeOnDeleteTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		public String item;
	}
	
	public static class Holder {
		public Item[] items;
	}
	
	public void testNoAccidentalDeletes() throws Exception {
		assertNoAccidentalDeletes(true, true);
		assertNoAccidentalDeletes(true, false);
		assertNoAccidentalDeletes(false, true);
		assertNoAccidentalDeletes(false, false);
	}
	
	private void assertNoAccidentalDeletes(boolean cascadeOnUpdate, boolean cascadeOnDelete) throws Exception {
		deleteAll(Holder.class);
		deleteAll(Item.class);
		
		ObjectClass oc = fixture().config().objectClass(Holder.class);
		oc.cascadeOnDelete(cascadeOnDelete);
		oc.cascadeOnUpdate(cascadeOnUpdate);
		
		reopen();
		
		Item item = new Item();
		Holder holder = new Holder();
		holder.items = new Item[]{ item };
		db().store(holder);
		db().commit();
		
		holder.items[0].item = "abrakadabra";
		db().store(holder);
		if(! cascadeOnDelete && ! cascadeOnUpdate){
			// the only case, where we don't cascade
			db().store(holder.items[0]);
		}
		
		Assert.areEqual(1, countOccurences(Item.class));
		db().commit();
		Assert.areEqual(1, countOccurences(Item.class));
	}
}
