/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import db4ounit.*;

/**
 * @exclude
 */
public class TAIntTestCase extends TAItemTestCaseBase {

	public static void main(String[] args) {
		new TAIntTestCase().runAll();
	}

	protected Object createItem() throws Exception {
		TAIntItem item = new TAIntItem();
		item.value = 42;
		item.i = new Integer(1);
		item.obj = new Integer(2);
		return item;
	}

	protected void assertItemValue(Object obj) throws Exception {
		TAIntItem item = (TAIntItem) obj;
		Assert.areEqual(42, item.value());
		Assert.areEqual(new Integer(1), item.integerValue());
		Assert.areEqual(new Integer(2), item.object());
	}

}
