/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

public interface PerformanceComparisonStrategy {

	boolean acceptableDiff(long current, long other);

}
