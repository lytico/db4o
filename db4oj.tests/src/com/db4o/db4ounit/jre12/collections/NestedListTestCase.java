/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 * @exclude
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class NestedListTestCase extends AbstractDb4oTestCase{
    
	public static class Item {
        
        public List list;
        
        public boolean equals(Object obj) {
            if(! (obj instanceof Item)){
                return false;
            }
            Item otherItem = (Item) obj;
            if(list == null){
                return otherItem.list == null;
            }
            return list.equals(otherItem.list);
        }
        
    }
    
    protected void store() throws Exception {
        store(storedItem());
    }

    private Item storedItem() {
        Item item = new Item();
        item.list = newNestedList(10);
        return item;
    }
    
    private List newNestedList(int depth) {
        List list = new ArrayList();
        list.add("StringItem");
        if(depth > 0){
            list.add(newNestedList(depth - 1));
        }
        return list;
    }

    public void testNestedList(){
        Item item = (Item) retrieveOnlyInstance(Item.class);
        db().activate(item, Integer.MAX_VALUE);
        Assert.areEqual(storedItem(), item);
    }

}
