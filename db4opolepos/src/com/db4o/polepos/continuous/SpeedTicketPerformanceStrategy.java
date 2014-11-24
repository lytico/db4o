/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

public class SpeedTicketPerformanceStrategy implements PerformanceComparisonStrategy {

	private final double _percentageThreshold;
	
	public SpeedTicketPerformanceStrategy(double percentageThreshold) {
		_percentageThreshold = percentageThreshold;
	}

	public boolean acceptableDiff(long current, long other) {
		if(current <= other) {
			return true;
		}
		long diff = current - other;
		double percentageValue = (other * _percentageThreshold) / 100;
		return diff < percentageValue;
	}

}
