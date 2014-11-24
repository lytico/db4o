/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.assorted;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class StoreComparableFieldTestCase extends AbstractDb4oTestCase{
    
    public static class Item{
        
        public Comparable _name;

        public Item(String name_) {
            _name = name_;
        }
        
    }
    
    protected void store() throws Exception {
        store(new Item("one"));
        store(new Item("two"));
    }
    
    public void testStoreStringInComparable(){
        Query query = newQuery(Item.class);
        query.descend("_name").constrain("one");
        ObjectSet objectSet = query.execute();
        Assert.areEqual(1, objectSet.size());
        Item item = (Item) objectSet.next();
        Assert.areEqual("one", item._name);
    }

}
