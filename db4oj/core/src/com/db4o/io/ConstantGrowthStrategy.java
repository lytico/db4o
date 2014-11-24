/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.io;

/**
 * Strategy for file/byte array growth by a constant factor
 */
public class ConstantGrowthStrategy implements GrowthStrategy {	
	private final int _growth;
	
	/**
	 * @param growth The constant growth size
	 */
	public ConstantGrowthStrategy(int growth) {
		_growth = growth;
	}
	
	/**
	 * returns the incremented size after the growth 
	 * strategy has been applied
	 * @param curSize the original size
	 * @return the new size
	 */
	public long newSize(long curSize, long requiredSize) {
		long newSize = curSize;
		while(newSize < requiredSize) {
			newSize += _growth;
		}
		return newSize;
	}
}
