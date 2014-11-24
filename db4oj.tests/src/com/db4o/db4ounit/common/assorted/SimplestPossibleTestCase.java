/* Copyright (C) 2006 - 2007 Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class SimplestPossibleTestCase extends AbstractDb4oTestCase {
    
    public static void main(String[] args) {
        new SimplestPossibleTestCase().runNetworking();
    }
    
    protected void store() {
        db().store(new Item("one"));
    }
    
    public void test(){
        Query q = db().query();
        q.constrain(Item.class);
        q.descend("name").constrain("one");
        ObjectSet objectSet = q.execute();
        Item item = (Item) objectSet.next();
        Assert.isNotNull(item);
        Assert.areEqual("one", item.getName());
    }
    
    public static class Item{
    	
        public String name;
        
        public Item() {
        }
        
        public Item(String name_) {
            this.name = name_;
        }

        public String getName() {
            return name;
        }

    }

}
