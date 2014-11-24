/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.nonta;

import db4ounit.*;

/**
 * @exclude
 */
public class NonTAStringTestCase extends NonTAItemTestCaseBase {

	public static void main(String[] args) {
		new NonTAStringTestCase().runAll();
	}

    protected void assertItemValue(Object obj) {
        StringItem item = (StringItem) obj;
        Assert.areEqual("42", item.value());
        Assert.areEqual("hello", item.object());
    }

    protected Object createItem() {
        StringItem item = new StringItem();
        item.value = "42";
        item.obj = "hello";
        return item;
    }
    
}
