/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.nonta;

import db4ounit.*;

/**
 * @exclude
 */
public class NonTAIntTestCase extends NonTAItemTestCaseBase {

	public static void main(String[] args) {
		new NonTAIntTestCase().runAll();
	}

    protected void assertItemValue(Object obj) {
        IntItem item = (IntItem) obj;
        Assert.areEqual(42, item.value());
        Assert.areEqual(new Integer(1), item.integerValue());
        Assert.areEqual(new Integer(2), item.object());
    }

    protected Object createItem() {
        IntItem item = new IntItem();
        item.value = 42;
        item.i = new Integer(1);
        item.obj = new Integer(2);
        return item;
    }

}
