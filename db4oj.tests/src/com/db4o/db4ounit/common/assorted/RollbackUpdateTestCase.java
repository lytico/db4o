/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class RollbackUpdateTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] args) {
		new RollbackUpdateTestCase().runNetworking();
	}

	protected void store() {
		store(new SimpleObject("hello", 1));
	}

	public void test() {
		ExtObjectContainer oc1 = openNewSession();
		ExtObjectContainer oc2 = openNewSession();
		ExtObjectContainer oc3 = openNewSession();
		try {
			SimpleObject o1 = (SimpleObject) retrieveOnlyInstance(oc1,
					SimpleObject.class);
			o1.setS("o1");
			oc1.store(o1);
			SimpleObject o2 = (SimpleObject) retrieveOnlyInstance(oc2,
					SimpleObject.class);
			Assert.areEqual("hello", o2.getS());

			oc1.rollback();
			o2 = (SimpleObject) retrieveOnlyInstance(oc2, SimpleObject.class);
			oc2.refresh(o2, Integer.MAX_VALUE);
			Assert.areEqual("hello", o2.getS());

			oc1.commit();
			o2 = (SimpleObject) retrieveOnlyInstance(oc2, SimpleObject.class);
			Assert.areEqual("hello", o2.getS());

			oc1.store(o1);
			oc1.commit();
			oc2.refresh(o2, Integer.MAX_VALUE);
			o2 = (SimpleObject) retrieveOnlyInstance(oc2, SimpleObject.class);
			Assert.areEqual("o1", o2.getS());

			SimpleObject o3 = (SimpleObject) retrieveOnlyInstance(oc3,
					SimpleObject.class);
			Assert.areEqual("o1", o3.getS());
		} finally {
			oc1.close();
			oc2.close();
			oc3.close();
		}
	}

}
