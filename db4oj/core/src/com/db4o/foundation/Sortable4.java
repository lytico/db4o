/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public interface Sortable4 {
	
	public int size();
	
	public int compare(int leftIndex, int rightIndex);
	
	public void swap(int leftIndex, int rightIndex);

}
