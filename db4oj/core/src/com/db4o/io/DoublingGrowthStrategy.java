/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.io;

/**
 * Strategy for file/byte array growth that will always double the current size
 */
public class DoublingGrowthStrategy implements GrowthStrategy {
	public long newSize(long curSize, long requiredSize) {
		if(curSize == 0) {
			return requiredSize;
		}
		long newSize = curSize;
		while(newSize < requiredSize) {
			newSize *= 2;
		}
		return newSize;
	}
}
