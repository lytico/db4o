/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.db4ounit.common.persistent.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ReadObjectQBETestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new ReadObjectQBETestCase().runConcurrency();
	}

	private static String testString = "simple test string";

	protected void store() {
		for (int i = 0; i < threadCount(); i++) {
			store(new SimpleObject(testString + i, i));
		}
	}
	
	public void concReadSameObject(ExtObjectContainer oc) throws Exception {
		int mid = threadCount() / 2;
		SimpleObject example = new SimpleObject(testString + mid, mid);
		ObjectSet result = oc.queryByExample(example);
		Assert.areEqual(1, result.size());
		Assert.areEqual(example, result.next());
	}

	public void concReadDifferentObject(ExtObjectContainer oc, int seq)
			throws Exception {
		SimpleObject example = new SimpleObject(testString + seq, seq);
		ObjectSet result = oc.queryByExample(example);
		Assert.areEqual(1, result.size());
		Assert.areEqual(example, result.next());
	}

}
