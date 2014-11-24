/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class Queue4TestCaseBase implements TestCase {

	public Queue4TestCaseBase() {
		super();
	}

	protected void assertIterator(Queue4 queue, String[] data, int size) {
		Iterator4 iter = queue.iterator();
		for (int idx = 0; idx < size; idx++) {
			Assert.isTrue(iter.moveNext(),
					"should be able to move in iteration #" + idx + " of "
							+ size);
			Assert.areEqual(data[idx], iter.current());
		}
		Assert.isFalse(iter.moveNext());
	}

}