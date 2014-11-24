/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class NullElementsInArrayTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new NullElementsInArrayTestCase().runSolo();
    }
    
    /**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Item {
        
        public Integer[] array;
        
    }
    
    /**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ItemArrayHolder {
        
        public NamedItem[] child;
        
    }
    
    /**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class NamedItem {
        
        public String name;
        
        public NamedItem(){
            
        }
        
        public NamedItem(String name_){
            name = name_;
        }
        
    }
    
    private static Integer[] DATA = new Integer[]{ new Integer(1), null, new Integer(2) };
    
    protected void store() throws Exception {
        Item item = new Item();
        item.array = DATA;
        store(item);
        
        ItemArrayHolder holder = new ItemArrayHolder();
        holder.child = new NamedItem[] {
            new NamedItem("one"),
            null,
            new NamedItem("two"),
        };
        store(holder);
        
    }
    
    public void testRetrieve(){
        Item item = (Item) retrieveOnlyInstance(Item.class);
        ArrayAssert.areEqual(DATA, item.array);
    }
    
    public void testQueryIntegerNull(){
        Query query = newQuery(Item.class);
        query.descend("array").constrain(null);
        Assert.areEqual(1, query.execute().size());
    }
    
    public void testQuerySubNode() {
        Query query = newQuery(ItemArrayHolder.class);
        Query itemQuery = query.descend("child");
        itemQuery.descend("name").constrain("one");
        ObjectSet objectSet = itemQuery.execute();
        Assert.areEqual(2, objectSet.size());
    }
    
    

}
