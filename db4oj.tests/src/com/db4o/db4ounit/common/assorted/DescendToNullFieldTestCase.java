/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class DescendToNullFieldTestCase extends AbstractDb4oTestCase{
	
	private static int COUNT = 2;

	public static class ParentItem{
		
		public String _name;
		
		public ChildItem one;
		
		public ChildItem two;

		public ParentItem(String name, ChildItem child1, ChildItem child2) {
			_name = name;
			one = child1;
			two = child2;
		}
	}
	
	public static class ChildItem{
		
		public String _name;

		public ChildItem(String name) {
			_name = name;
		}
		
	}
	
	protected void store() throws Exception {
		for (int i = 0; i < COUNT; i++) {
			store(new ParentItem("one", new ChildItem("one"), null));
		}
		for (int i = 0; i < COUNT; i++) {
			store(new ParentItem("two", null, new ChildItem("two")));
		}

	}
	
	public void test(){
		assertResults("one");
		assertResults("two");
	}
	
	private void assertResults(String name){
		Query query = newQuery(ParentItem.class);
		query.descend(name).descend("_name").constrain(name);
		ObjectSet objectSet = query.execute();
		Assert.areEqual(COUNT, objectSet.size());
		while(objectSet.hasNext()){
			ParentItem parentItem = (ParentItem) objectSet.next();
			Assert.areEqual(name, parentItem._name);
		}
	}

}
