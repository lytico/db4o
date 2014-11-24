/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.ta.collections;

import java.util.*;

import com.db4o.db4ounit.common.ta.*;
import com.db4o.db4ounit.common.ta.nonta.*;
import com.db4o.db4ounit.common.ta.ta.*;

import db4ounit.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TAMapTestCase extends ItemTestCaseBase {

    public static void main(String[] args) {
        new TAMapTestCase().runAll();
    }
    
    protected void assertItemValue(Object obj) throws Exception {
        HashMap item = (HashMap) obj;
        IntItem intItem = (IntItem) item.get(IntItem.class.getName());
        TAIntItem taIntItem = (TAIntItem) item.get(TAIntItem.class.getName());
        
        Assert.areEqual(100, intItem.value());
        Assert.areEqual(new Integer(200), intItem.integerValue());
        Assert.areEqual(new Integer(300), intItem.object());

        Assert.areEqual(100, taIntItem.value());
        Assert.areEqual(new Integer(200), taIntItem.integerValue());
        Assert.areEqual(new Integer(300), taIntItem.object());
    }

    protected void assertRetrievedItem(Object obj) throws Exception {
        HashMap item = (HashMap) obj;
        IntItem intItem = (IntItem) item.get(IntItem.class.getName());
        TAIntItem taIntItem = (TAIntItem) item.get(TAIntItem.class.getName());
        
        Assert.isNotNull(intItem);
        Assert.isNotNull(taIntItem);
        
        Assert.areEqual(100, intItem.value);
        Assert.areEqual(new Integer(200), intItem.i);
        Assert.areEqual(new Integer(300), intItem.obj);

        Assert.areEqual(0, taIntItem.value);
        isPrimitiveNull(taIntItem.i);
        Assert.isNull(taIntItem.obj);
    }

    protected Object createItem() throws Exception {
        IntItem intItem = new IntItem();
        intItem.value = 100;
        intItem.i = new Integer(200);
        intItem.obj = new Integer(300);
        
        TAIntItem taIntItem = new TAIntItem();
        taIntItem.value = 100;
        taIntItem.i = new Integer(200);
        taIntItem.obj = new Integer(300);
        
        HashMap item = new HashMap();
        item.put(intItem.getClass().getName(), intItem);
        item.put(taIntItem.getClass().getName(), taIntItem);
        return item;
    }

}
