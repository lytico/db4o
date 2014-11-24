/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadedDeleteUpdate extends AbstractDb4oTestCase {
	
	public static class ParentItem {
		public Object child;
	}

	public static class ChildItem {
		public Object parent1;
		public Object parent2;
	}
	
	public static void main(String[] arguments) {
//		new CascadedDeleteUpdate().runSolo();
		new CascadedDeleteUpdate().runNetworking();
	}
	
	protected void configure(Configuration config) {
		config.objectClass(ParentItem.class).cascadeOnDelete(true);
	}
	
	protected void store() {
		ParentItem parentItem1 = new ParentItem();
		ParentItem parentItem2 = new ParentItem();
		
		ChildItem child = new ChildItem();
		child.parent1 = parentItem1;
		child.parent2 = parentItem2;
		parentItem1.child = child; 
		parentItem2.child = child;
		
		db().store(parentItem1);
	}
	
	public void testAllObjectStored() throws Exception{
		assertAllObjectStored();
	}
	
	public void testUpdate() throws Exception{
		Query q = newQuery(ParentItem.class);
		ObjectSet objectSet = q.execute();
		while(objectSet.hasNext()){
			db().store(objectSet.next());
		}
		db().commit();
		assertAllObjectStored();
	}
	
	private void assertAllObjectStored() throws Exception{
		reopen();
		Query q = newQuery(ParentItem.class);
		ObjectSet objectSet = q.execute();
		while(objectSet.hasNext()){
			ParentItem parentItem = (ParentItem) objectSet.next();
			db().refresh(parentItem, 3);
			Assert.isNotNull(parentItem.child);
		}
	}
	
}
