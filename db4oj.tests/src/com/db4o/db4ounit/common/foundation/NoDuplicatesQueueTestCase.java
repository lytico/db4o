/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class NoDuplicatesQueueTestCase implements TestLifeCycle {

	private Queue4 _queue;

	public void test() {
		_queue.add("A");
		_queue.add("B");
		_queue.add("B");
		_queue.add("A");
		Assert.areEqual("A", _queue.next());
		Assert.areEqual("B", _queue.next());
		Assert.isFalse(_queue.hasNext());
	}
	
	public void setUp() throws Exception {
		_queue = new NoDuplicatesQueue(new NonblockingQueue());
	}

	public void tearDown() throws Exception {
		_queue = null;
	}
}
