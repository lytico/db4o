/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.staging;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.extensions.*;


public class LazyQueryDeleteTestCase extends AbstractDb4oTestCase {
	
	private static final int COUNT = 3;
	
	public static class Item{
		
		public String _name;

		public Item(String name) {
			_name = name;
		}
		
	}
	
	protected void configure(Configuration config) {
		config.queries().evaluationMode(QueryEvaluationMode.LAZY);
	}
	
	protected void store() throws Exception {
		for (int i = 0; i < COUNT; i++) {
			store(new Item(new Integer(i).toString()));
			db().commit();
		}
	}
	
	public void test(){
		ObjectSet objectSet = newQuery(Item.class).execute();
		for (int i = 0; i < COUNT; i++) {
			db().delete(objectSet.next());
			db().commit();
		}
	}
	
	public static void main(String[] arguments) {
		new LazyQueryDeleteTestCase().runSolo();
	}

}
