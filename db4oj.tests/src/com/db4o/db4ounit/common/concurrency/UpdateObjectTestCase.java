/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.db4ounit.common.persistent.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class UpdateObjectTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new UpdateObjectTestCase().runConcurrency();
	}

	private static String testString = "simple test string";

	private static int COUNT = 100;

	protected void store() throws Exception {
		for (int i = 0; i < COUNT; i++) {
			store(new SimpleObject(testString + i, i));
		}

	}

	public void concUpdateSameObject(ExtObjectContainer oc, int seq)
			throws Exception {
		Query query = oc.query();
		query.descend("_s").constrain(testString + COUNT / 2);
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
		SimpleObject o = (SimpleObject) result.next();
		o.setI(COUNT + seq);
		oc.store(o);

	}

	public void checkUpdateSameObject(ExtObjectContainer oc) throws Exception {
		Query query = oc.query();
		query.descend("_s").constrain(testString + COUNT / 2);
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
		SimpleObject o = (SimpleObject) result.next();
		int i = o.getI();
		Assert.isTrue(COUNT <= i && i < COUNT + threadCount());

	}

	public void concUpdateDifferentObject(ExtObjectContainer oc, int seq)
			throws Exception {
		Query query = oc.query();
		query.descend("_s").constrain(testString + seq).and(
				query.descend("_i").constrain(new Integer(seq)));
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
		SimpleObject o = (SimpleObject) result.next();
		o.setI(seq + COUNT);
		oc.store(o);
	}

	public void checkUpdateDifferentObject(ExtObjectContainer oc)
			throws Exception {

		ObjectSet result = oc.query(SimpleObject.class);
		Assert.areEqual(COUNT, result.size());
		while (result.hasNext()) {
			SimpleObject o = (SimpleObject) result.next();
			int i = o.getI();
			if (i >= COUNT) {
				i -= COUNT;
			}
			Assert.areEqual(testString + i, o.getS());
		}

	}

}
