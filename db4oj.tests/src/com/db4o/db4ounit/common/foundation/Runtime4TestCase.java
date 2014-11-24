/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class Runtime4TestCase implements TestCase {

	public void testLoopWithTimeoutReturnsWhenBlockIsFalse() {
		
		StopWatch watch = new AutoStopWatch();		
		Runtime4.retry(500, new Closure4<Boolean>() {
			public Boolean run() {
				return true;
			}
		});
		Assert.isSmaller(500, watch.peek());
	}
	
	public void testLoopWithTimeoutReturnsAfterTimeout() {
		StopWatch watch = new AutoStopWatch();		
		Runtime4.retry(500, new Closure4<Boolean>() {
			public Boolean run() {
				return false;
			}
		});
		watch.stop();
		Assert.isGreaterOrEqual(500, watch.elapsed());
	}
	
}
