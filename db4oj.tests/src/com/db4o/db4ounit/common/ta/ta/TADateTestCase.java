/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import java.util.*;

import db4ounit.*;

public class TADateTestCase extends TAItemTestCaseBase {

    public static Date first = new Date(1195401600000L);

    public static void main(String[] args) {
        new TADateTestCase().runAll();
    }

    protected void assertItemValue(Object obj) throws Exception {
        TADateItem item = (TADateItem) obj;
        Assert.areEqual(first, item.getUntyped());
        Assert.areEqual(first, item.getTyped());
    }

    protected void assertRetrievedItem(Object obj) throws Exception {
        TADateItem item = (TADateItem) obj;
        Assert.isNull(item._untyped);
        Assert.areEqual(emptyValue(),item._typed);
    }
    
    private Object emptyValue() {
        return db().reflector().forClass(Date.class).nullValue();
    }

    protected Object createItem() throws Exception {
        TADateItem item = new TADateItem();
        item._typed = first;
        item._untyped = first;
        return item;
    }
}
