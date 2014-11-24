/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.extensions.*;

public class CascadeDeleteFalseTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] args) {
		new CascadeDeleteFalseTestCase().runConcurrency();
	}

	public static class Item {
		public CascadeDeleteFalseHelper h1;

		public CascadeDeleteFalseHelper h2;

		public CascadeDeleteFalseHelper h3;
	}

	protected void configure(Configuration config) {
		config.objectClass(Item.class).cascadeOnDelete(true);
		config.objectClass(Item.class).objectField("h3").cascadeOnDelete(false);
	}

	protected void store() {
		Item item = new Item();
		item.h1 = new CascadeDeleteFalseHelper();
		item.h2 = new CascadeDeleteFalseHelper();
		item.h3 = new CascadeDeleteFalseHelper();
		store(item);
	}

	public void concDelete(ExtObjectContainer oc) throws Exception {
		ObjectSet os = oc.query(Item.class);
		if (os.size() == 0) { // the object has been deleted
			return;
		}
		if(! os.hasNext()){
			// object can be deleted after query 
			return;
		}
		Item cdf = (Item) os.next();
		// sleep 1000 ms, waiting for other threads.
		// Thread.sleep(500);
		oc.delete(cdf);
		oc.commit();
		assertOccurrences(oc, Item.class, 0);
		assertOccurrences(oc, CascadeDeleteFalseHelper.class, 1);
	}

	public void checkDelete(ExtObjectContainer oc) {
		assertOccurrences(oc, CascadeDeleteFalseTestCase.class, 0);
		assertOccurrences(oc, CascadeDeleteFalseHelper.class, 1);
	}

	public static class CascadeDeleteFalseHelper {

	}
}
