/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

/**
 * Non boxing/unboxing version of {@link java.util.Comparator} for
 * faster id comparisons.
 */
public interface IntComparator {
	
	int compare(int x, int y);

}
