/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ConstructorNotRequiredTestCase extends AbstractDb4oTestCase {
	
    public static class Item {
    	
    	public String name;
        
        public Item(Object obj){
            if(obj == null){
                throw new RuntimeException();
            }
        }
        
        public static Item newItem(){
            return new Item(new Object());
        }
    }
    
    protected void store() throws Exception {
    	Item item = Item.newItem();
    	item.name = "one";
    	store(item);
    }
    
    public void test(){
    	Item item = (Item) retrieveOnlyInstance(Item.class);
    	Assert.areEqual("one", item.name);
    }
}
