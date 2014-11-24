/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda;

import java.util.*;

import com.db4o.query.*;

import db4ounit.extensions.*;

/**
 * @exclude
 */
public class CollectIdTestCase extends AbstractDb4oTestCase{
	
	public static class ListHolder {
		
		public List _list;
		
	}
	
	public static class Parent {
		
		public Child _child;
		
	}
	
	public static class Child {
		
		public String _name;
		
	}
	
	
	protected void store() throws Exception {
		ListHolder holder = new ListHolder();
		holder._list = new ArrayList();
		Parent parent = new Parent();
		holder._list.add(parent);
		parent._child = new Child();
		parent._child._name = "child";
		store(holder);
	}
	
	public void test(){
		Query query = newQuery(ListHolder.class);
		Query qList = query.descend("_list");
		// qList.execute();
		Query qChild = qList.descend("_child");
		qChild.execute();
		// Query qName = qList.descend("_name");
		
	}

}
