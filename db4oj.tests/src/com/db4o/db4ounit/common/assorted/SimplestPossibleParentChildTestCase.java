/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;


public class SimplestPossibleParentChildTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new SimplestPossibleParentChildTestCase().runAll();
    }
    
    public static class ParentItem{
        
        public Object child;
    }
    
    public static class ChildItem {
        
    }
    
    protected void store() throws Exception {
        ParentItem parentItem = new ParentItem();
        parentItem.child = new ChildItem();
        store(parentItem);
    }
    
    public void test(){
        ParentItem parentItem = (ParentItem) retrieveOnlyInstance(ParentItem.class);
        Assert.isInstanceOf(ChildItem.class, parentItem.child);
    }

}
