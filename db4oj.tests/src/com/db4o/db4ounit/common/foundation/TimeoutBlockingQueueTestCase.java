/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class TimeoutBlockingQueueTestCase extends Queue4TestCaseBase {
	
	public void testTimeoutNext() {
		
		TimeoutBlockingQueue4<Object> queue = new TimeoutBlockingQueue<Object>(300);
		
		queue.pause();
		
		queue.add(new Object());
		
		queue.check();
		
		Assert.isNull(queue.tryNext());
		
		Runtime4.sleepThrowsOnInterrupt(500);
		queue.check();
		
		Assert.isNotNull(queue.tryNext());
		
	}
	
}
