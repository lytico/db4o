/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.test.continuous;

import com.db4o.polepos.continuous.*;

import db4ounit.*;

public class SpeedTicketPerformanceStrategyTestCase implements TestCase {

	public void test() {
		PerformanceComparisonStrategy strategy = new SpeedTicketPerformanceStrategy(10.0);
		Assert.isTrue(strategy.acceptableDiff(1000, 1000));
		Assert.isTrue(strategy.acceptableDiff(500, 1000));
		Assert.isTrue(strategy.acceptableDiff(1099, 1000));
		Assert.isFalse(strategy.acceptableDiff(1100, 1000));
		Assert.isFalse(strategy.acceptableDiff(1101, 1000));
	}
	
}
