/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DeleteUpdateTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new DeleteUpdateTestCase().runNetworking();
	}

	protected void store() {
		store(new SimpleObject("hello", 1));
	}

	/*
	 * delete - set - commit delete - commit set
	 */
	public void _testDS() {
		ExtObjectContainer oc1 = openNewSession();
		ExtObjectContainer oc2 = openNewSession();
		ExtObjectContainer oc3 = openNewSession();
		try {
			SimpleObject o1 = (SimpleObject) retrieveOnlyInstance(oc1,
					SimpleObject.class);
			oc1.delete(o1);
			SimpleObject o2 = (SimpleObject) retrieveOnlyInstance(oc2,
					SimpleObject.class);
			Assert.areEqual("hello", o2.getS());
			o2.setS("o2");
			oc2.store(o2);

			oc1.commit();
			oc2.commit();

			o1 = (SimpleObject) retrieveOnlyInstance(oc1, SimpleObject.class);
			oc1.refresh(o1, Integer.MAX_VALUE);
			Assert.areEqual("o2", o1.getS());

			o2 = (SimpleObject) retrieveOnlyInstance(oc2, SimpleObject.class);
			oc2.refresh(o2, Integer.MAX_VALUE);
			Assert.areEqual("o2", o2.getS());

			SimpleObject o3 = (SimpleObject) retrieveOnlyInstance(oc3,
					SimpleObject.class);
			oc1.refresh(o1, Integer.MAX_VALUE);
			Assert.areEqual("o2", o3.getS());

		} finally {
			oc1.close();
			oc2.close();
			oc3.close();
		}

	}

	/*
	 * delete - set - commit set - commit delete
	 */
	public void testSD() {
		ExtObjectContainer oc1 = openNewSession();
		ExtObjectContainer oc2 = openNewSession();
		ExtObjectContainer oc3 = openNewSession();
		try {
			SimpleObject o1 = (SimpleObject) retrieveOnlyInstance(oc1,
					SimpleObject.class);
			oc1.delete(o1);
			SimpleObject o2 = (SimpleObject) retrieveOnlyInstance(oc2,
					SimpleObject.class);
			Assert.areEqual("hello", o2.getS());
			o2.setS("o2");
			oc2.store(o2);

			oc2.commit();
			oc1.commit();

			assertOccurrences(oc1, SimpleObject.class, 0);
			assertOccurrences(oc2, SimpleObject.class, 0);
			assertOccurrences(oc3, SimpleObject.class, 0);

		} finally {
			oc1.close();
			oc2.close();
			oc3.close();
		}

	}


}
