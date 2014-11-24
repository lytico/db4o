/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.io;

/**
 * Strategy for file/byte array growth.
 */
public interface GrowthStrategy {
	
	/**
	 * returns the incremented size after the growth 
	 * strategy has been applied
	 * @param curSize the original size
	 * @return the new size, after the growth strategy has been
	 * applied, must be bigger than curSize
	 */

	long newSize(long curSize, long requiredSize);
}
