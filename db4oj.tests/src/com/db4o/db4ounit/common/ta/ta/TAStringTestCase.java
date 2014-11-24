/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import db4ounit.*;

/**
 * @exclude
 */
public class TAStringTestCase extends TAItemTestCaseBase {

	public static void main(String[] args) {
		new TAStringTestCase().runAll();
	}

	protected Object createItem() throws Exception {
		TAStringItem item = new TAStringItem();
		item.value = "42";
		item.obj = "hello";
		return item;
	}

	protected void assertItemValue(Object obj) throws Exception {
		TAStringItem item = (TAStringItem) obj;
		Assert.areEqual("42", item.value());
		Assert.areEqual("hello", item.object());
	}

	protected void assertRetrievedItem(Object obj) {
		TAStringItem item = (TAStringItem) obj;
		Assert.isNull(item.value);
		Assert.isNull(item.obj);
	}

}
