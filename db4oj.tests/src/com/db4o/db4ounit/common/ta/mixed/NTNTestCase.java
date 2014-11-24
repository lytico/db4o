/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.mixed;

import com.db4o.db4ounit.common.ta.ItemTestCaseBase;
import com.db4o.db4ounit.common.ta.LinkedList;

import db4ounit.Assert;

/**
 * @exclude
 */
public class NTNTestCase extends ItemTestCaseBase {
	
	public static void main(String[] args) {
		new NTNTestCase().runAll();
	}
	
	protected Object createItem() throws Exception {
		return new NTNItem(42);
	}

	protected void assertRetrievedItem(Object obj) throws Exception {
		NTNItem item = (NTNItem) obj;
		Assert.isNotNull(item.tnItem);
		Assert.isNull(item.tnItem.list);
	}
		
	protected void assertItemValue(Object obj) throws Exception {
		NTNItem item = (NTNItem) obj;
		Assert.areEqual(LinkedList.newList(42), item.tnItem.value());
	}
	
	public void testDeactivateDepth() throws Exception {
		NTNItem item = (NTNItem) retrieveOnlyInstance();
		TNItem tnItem = item.tnItem;
		tnItem.value();
		Assert.isNotNull(tnItem.list);
		// item.tnItem.list
		db().deactivate(item, 2);
		// FIXME: failure 
		// Assert.isNull(tnItem.list);
		
		db().activate(item, 42);
		db().deactivate(item, 10);
		Assert.isNull(tnItem.list);
	}

}
