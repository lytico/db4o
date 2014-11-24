/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

/**
 * marker interface for Indexable4, if a check for null is necessary
 * in BTreeRangeSingle#firstBTreePointer() 
 * @exclude
 */
public interface CanExcludeNullInQueries {
	
	boolean excludeNull();
}
