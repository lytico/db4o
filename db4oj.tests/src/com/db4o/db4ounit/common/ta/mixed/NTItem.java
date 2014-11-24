/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

/**
 * @exclude
 */
public class NTItem {
	public TItem tItem;
	
	public NTItem() {
		
	}
	
	public NTItem(int value) {
		tItem = new TItem(value);
	}	
}
