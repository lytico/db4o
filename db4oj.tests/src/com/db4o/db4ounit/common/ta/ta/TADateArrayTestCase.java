/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import java.util.*;

import db4ounit.*;

public class TADateArrayTestCase extends TAItemTestCaseBase {

    public static final Date[] data = {
        new Date(0),
        new Date(1),
        new Date(1191972104500L),
    };

    public static void main(String[] args) {
        new TADateArrayTestCase().runAll();
    }
    
    protected void assertItemValue(Object obj) throws Exception {
        TADateArrayItem item = (TADateArrayItem) obj;
        for (int i = 0; i < data.length; i++) {
            Assert.areEqual(data[i], item.getTyped()[i]);
            Assert.areEqual(data[i], (Date) item.getUntyped()[i]);
        }
    }

    protected Object createItem() throws Exception {
        TADateArrayItem item = new TADateArrayItem();
        item._typed = new Date[data.length];
        item._untyped = new Object[data.length];
        System.arraycopy(data, 0, item._typed, 0, data.length);
        System.arraycopy(data, 0, item._untyped, 0, data.length);
        return item;
    }

}
