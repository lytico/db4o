/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.regression;

import com.db4o.*;
import com.db4o.db4ounit.common.assorted.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class Case1207TestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) throws Exception {
		new Case1207TestCase().runNetworking();
	}

	/*
	 * client 1: set and commit client 2: set and rollback
	 */
	public void test() throws Exception {
		ObjectContainer oc1 = openNewSession();
		ObjectContainer oc2 = openNewSession();
		ObjectContainer oc3 = openNewSession();
		try {
			for (int i = 0; i < 1000; i++) {
				SimpleObject obj1 = new SimpleObject("oc " + i, i);
				SimpleObject obj2 = new SimpleObject("oc2 " + i, i);
				oc1.store(obj1);
				oc2.store(obj2);
				oc2.rollback();
				obj2 = new SimpleObject("oc2.2 " + i, i);
				oc2.store(obj2);
			}
			oc1.commit();
			oc2.rollback();
			Assert.areEqual(1000, oc1.query(SimpleObject.class).size());
			Assert.areEqual(1000, oc2.query(SimpleObject.class).size());
			Assert.areEqual(1000, oc3.query(SimpleObject.class).size());
		} finally {
			oc1.close();
			oc2.close();
			oc3.close();
		}
	}
}
