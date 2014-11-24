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
public class TAArrayListTestCase extends ItemTestCaseBase {

    public static void main(String[] args) {
        new TAArrayListTestCase().runAll();
    }
    
    protected void assertItemValue(Object obj) throws Exception {
        ArrayList item = (ArrayList) obj;
        IntItem intItem = (IntItem) item.get(0);
        TAIntItem taIntItem = (TAIntItem) item.get(1);
        
        Assert.areEqual(100, intItem.value());
        Assert.areEqual(new Integer(200), intItem.integerValue());
        Assert.areEqual(new Integer(300), intItem.object());

        Assert.areEqual(100, taIntItem.value());
        Assert.areEqual(new Integer(200), taIntItem.integerValue());
        Assert.areEqual(new Integer(300), taIntItem.object());
        
    }
    
    protected void assertRetrievedItem(Object obj) throws Exception {
        ArrayList item = (ArrayList) obj;
        IntItem intItem = (IntItem) item.get(0);
        TAIntItem taIntItem = (TAIntItem) item.get(1);
        
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
        
        ArrayList item = new ArrayList();
        item.add(intItem);
        item.add(taIntItem);
        
        return item;
    }

}
