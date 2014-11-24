/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.db4ounit.common.persistent.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ReadObjectNQTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new ReadObjectNQTestCase().runConcurrency();
	}

	private static String testString = "simple test string";

	protected void store() throws Exception {
		for (int i = 0; i < threadCount(); i++) {
			store(new SimpleObject(testString + i, i));
		}
	}

	public void concReadSameObject(ExtObjectContainer oc) throws Exception {
		int mid = threadCount() / 2;
		final SimpleObject expected = new SimpleObject(testString + mid, mid);
		ObjectSet result = oc.query(new MyPredicate(expected));
		Assert.areEqual(1, result.size());
		Assert.areEqual(expected, result.next());
	}

	public void concReadDifferentObject(ExtObjectContainer oc, int seq)
			throws Exception {
		final SimpleObject expected = new SimpleObject(testString + seq, seq);
		ObjectSet result = oc.query(new MyPredicate(expected));
		Assert.areEqual(1, result.size());
		Assert.areEqual(expected, result.next());
	}

	public static class MyPredicate extends Predicate<SimpleObject> {
		public SimpleObject expected;

		public MyPredicate(SimpleObject o) {
			this.expected = o;
		}

		public boolean match(SimpleObject candidate) {
			return expected.equals(candidate);
		}
	}

}

