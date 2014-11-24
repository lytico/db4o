/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;


public class SimplestPossibleNullMemberTestCase extends AbstractDb4oTestCase{

    public static class Item{
        public Item _item;
    }
    
    protected void store() throws Exception {
        store(new Item());
    }
    
    public void test(){
        Item item = (Item) retrieveOnlyInstance(Item.class);
        Assert.isNull(item._item);
    }

}
