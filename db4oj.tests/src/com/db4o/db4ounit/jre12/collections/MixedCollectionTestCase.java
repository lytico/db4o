/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Remove(decaf.Platform.JDK11)
public class MixedCollectionTestCase extends AbstractDb4oTestCase{
	
	public static class ListHolder {
		
		public List list;
		
		public ListHolder(List list) {
			this.list = list;
		}
	}
	
	public static class Item {
		
		public String name;
		
		public Item(String name) {
			this.name = name;
		}
		
	}
	
	@Override
	protected void store() throws Exception {
		ArrayList list = new ArrayList();
		list.add("42");
		list.add(new Item("item"));
		ListHolder listHolder = new ListHolder(list);
		store(listHolder);
	}
	
	public void test(){
		ListHolder listHolder = retrieveOnlyInstance(ListHolder.class);
		Assert.areEqual("42", listHolder.list.get(0));
	}
	
	public void testStringQuery(){
		assertStringQuery(1, "42");
		assertStringQuery(0, "43");
	}
	
	public void testItemQuery(){
		assertItemQuery("item", 1);
		assertItemQuery("invalid", 0);
	}

	private void assertItemQuery(String constraint, int expected) {
		Query query = db().query();
		query.constrain(ListHolder.class);
		query.descend("list").descend("name").constrain(constraint);
		Assert.areEqual(expected, query.execute().size());
	}

	private void assertStringQuery(int expected, String valueToQuery) {
		Query query = db().query();
		query.constrain(ListHolder.class);
		query.descend("list").constrain(valueToQuery);
		Assert.areEqual(expected, query.execute().size());
	}


}
