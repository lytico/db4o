/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class ObjectConstructorTestCase extends AbstractDb4oTestCase {
    
    
    public static class Item {
        
        final String _name;
        
        public Item(String name){
            _name = name;
        }
    }
    
    public static class ItemConstructor implements ObjectConstructor{

        public Object onInstantiate(ObjectContainer container, Object storedObject) {
            return new Item((String)storedObject);
        }

        public void onActivate(ObjectContainer container, Object applicationObject,
            Object storedObject) {
            
        }

        public Object onStore(ObjectContainer container, Object applicationObject) {
            return ((Item)applicationObject)._name;
        }

        public Class storedClass() {
            return String.class;
        }
    }
    
    protected void configure(Configuration config) throws Exception {
        config.objectClass(Item.class).translate(new ItemConstructor());
    }
    
    protected void store(){
        store(new Item("one"));
    }
    
    public void test(){
        Item item = (Item) retrieveOnlyInstance(Item.class);
        Assert.areEqual("one", item._name);
    }
    

}
