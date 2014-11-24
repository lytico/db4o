/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

/**
 * @exclude
 */
public class NTNItem {
	
	public TNItem tnItem;

	public NTNItem() {

	}

	public NTNItem(int v) {
		tnItem = new TNItem(v);
	}

}
