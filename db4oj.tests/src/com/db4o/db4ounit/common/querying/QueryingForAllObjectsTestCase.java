/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class QueryingForAllObjectsTestCase extends AbstractDb4oTestCase {
	
	public static class Item{
		
	}
	
	@Override
	protected void store() throws Exception {
		for (int i = 0; i < 3; i++) {
			store(new Item());
		}
	}
	
	public void testConstrainObjectClass(){
		ObjectSet<Object> objectSet = db().query(Object.class);
		Assert.areEqual(3, objectSet.size());
	}
	
	public void testConstrainByNewObject(){
		ObjectSet<Object> objectSet = db().queryByExample(new Object());
		Assert.areEqual(3, objectSet.size());
	}


}
