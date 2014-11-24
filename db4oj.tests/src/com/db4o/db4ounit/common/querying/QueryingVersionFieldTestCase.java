/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class QueryingVersionFieldTestCase extends AbstractDb4oTestCase {
    
    public static void main(String[] arguments) {
        new QueryingVersionFieldTestCase().runAll();
    }
    
    public static class Item {
        
        public String name;
        
        public Item(String name_){
            name = name_;
        }
        
    }
    
    protected void configure(Configuration config) throws Exception {
        config.generateCommitTimestamps(true);
    }
    
    public void test(){
        
        storeItems(new String[] {"1", "2", "3"});
        db().commit();
        long initialTransactionVersionNumber = db().version();
        
        updateItem("2", "modified2");
        db().commit();
        long updatedTransactionVersionNumber = db().version();  
        
        Query q = db().query();
        q.constrain(Item.class);
        q.descend(VirtualField.COMMIT_TIMESTAMP).constrain(new Long(initialTransactionVersionNumber)).greater();
        
        // This part really isn't needed for this test case, but it shows, how changes
        // between two specific transaction commits can be queried.
        q.descend(VirtualField.COMMIT_TIMESTAMP).constrain(new Long(updatedTransactionVersionNumber)).smaller().equal();
        
        ObjectSet objectSet = q.execute();
        Assert.areEqual(1, objectSet.size());
        Item item = (Item) objectSet.next();
        Assert.areEqual("modified2", item.name);
        
        
        
    }

    private void updateItem(String originalName, String updatedName) {
        Item item = queryForItem(originalName);
        item.name = updatedName;
        store(item);
        
    }

    private Item queryForItem(String name) {
        Query q = newQuery(Item.class);
        q.descend("name").constrain(name);
        ObjectSet objectSet = q.execute();
        Assert.areEqual(1, objectSet.size());
        return (Item) objectSet.next();
    }

    private void storeItems(String[] names) {
        for (int i = 0; i < names.length; i++) {
            Item item = new Item(names[i]);
            store(item);
        }
    }

}
