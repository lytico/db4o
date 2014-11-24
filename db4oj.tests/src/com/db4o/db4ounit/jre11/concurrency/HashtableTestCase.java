/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.concurrency;

import java.util.*;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class HashtableTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		for (int i=0; i<50; ++i) {
			new HashtableTestCase().runEmbeddedConcurrency();
		}
	}
	
	private static long _id;
	
//	private static int run;
	
	protected void store() {
		Hashtable ht = new Hashtable();
		ht.put("key1", "val1");
		ht.put("key2", "val2");
		store(ht);
		_id = db().getID(ht);
	}
	
	public void conc(ExtObjectContainer oc) {
		Hashtable ht = (Hashtable) oc.getByID(_id);
		oc.activate(ht, Integer.MAX_VALUE);
		ht.put("key1", "updated1");
		String str = (String) ht.get("key2");
		Assert.areEqual("val2", str);
		oc.store(ht);
	}
	
}
