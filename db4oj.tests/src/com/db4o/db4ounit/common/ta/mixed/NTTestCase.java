/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

import com.db4o.db4ounit.common.ta.*;

import db4ounit.*;

/**
 * @exclude
 */
public class NTTestCase extends ItemTestCaseBase {
	
	public static void main(String[] args) {
		new NTTestCase().runAll();
	}
	
	protected Object createItem() throws Exception {
		return new NTItem(42);
	}

	protected void assertRetrievedItem(Object obj) throws Exception {
		NTItem item = (NTItem) obj;
		Assert.isNotNull(item.tItem);
		Assert.areEqual(0, item.tItem.value);
	}
		
	protected void assertItemValue(Object obj) throws Exception {
		NTItem item = (NTItem) obj;
		Assert.areEqual(42, item.tItem.value());
	}

	

}
