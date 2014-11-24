/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.nonta;

import java.util.*;

import db4ounit.*;

public class NonTADateTestCase extends NonTAItemTestCaseBase {

    public static Date first = new Date(1195401600000L);

    public static void main(String[] args) {
        new NonTADateTestCase().runAll();
    }

    protected void assertItemValue(Object obj) {
        DateItem item = (DateItem) obj;
        Assert.areEqual(first, item._untyped);
        Assert.areEqual(first, item._typed);
    }
    
    protected Object createItem() {
        DateItem item = new DateItem();
        item._typed = first;
        item._untyped = first;
        return item;
    }

}
