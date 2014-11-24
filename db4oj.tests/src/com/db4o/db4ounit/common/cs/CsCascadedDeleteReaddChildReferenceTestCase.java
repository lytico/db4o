/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.extensions.*;


public class CsCascadedDeleteReaddChildReferenceTestCase extends Db4oClientServerTestCase{
	
    public static class ItemParent {
    	
    	public Item child;
        
    }
    
    public static class Item {
        
        public String name;
        
        public Item(String name_){
            name = name_;
        }
    }
    
    
    @Override
    protected void configure(Configuration config) throws Exception {
    	config.objectClass(Item.class).objectField("name").indexed(true);
    	config.objectClass(ItemParent.class).objectField("child").indexed(true);
    }
    
    protected void store() throws Exception {
    	ItemParent parent = new ItemParent();
        Item child = new Item("child");
        parent.child = child;
		store(parent);
    }
    
    public void testDeleteReadd(){
        ExtObjectContainer client1 = db();
        ExtObjectContainer client2 = openNewSession();
        
        ItemParent parent1 = retrieveOnlyInstance(client1, ItemParent.class);
        ItemParent parent2 = retrieveOnlyInstance(client2, ItemParent.class);
        
        client1.delete(parent1);
        
        client1.commit();
        
        client2.ext().store(parent2, Integer.MAX_VALUE);
        client2.commit();
        client2.close();
        
        assertInstanceCountAndFieldIndexes(client1);
    }

    public void testRepeatedStore(){
        ExtObjectContainer client1 = db();
        ExtObjectContainer client2 = openNewSession();
        
        ItemParent parent1 = retrieveOnlyInstance(client1, ItemParent.class);
        ItemParent parent2 = retrieveOnlyInstance(client2, ItemParent.class);
        
        client1.ext().store(parent1, Integer.MAX_VALUE);
        
        client1.commit();
        
        client2.ext().store(parent2, Integer.MAX_VALUE);
        client2.commit();
        client2.close();
        
        assertInstanceCountAndFieldIndexes(client1);
    	
    }
    
	private void assertInstanceCountAndFieldIndexes(ExtObjectContainer client1) {
		ItemParent parent3 = retrieveOnlyInstance(client1, ItemParent.class);
        retrieveOnlyInstance(client1, Item.class);
        client1.refresh(parent3, Integer.MAX_VALUE);
        final long parentIdAfterUpdate = client1.getID(parent3);
        final long childIdAfterUpdate = client1.getID(parent3.child);
        
        new FieldIndexAssert(ItemParent.class, "child").assertSingleEntry(fileSession(), parentIdAfterUpdate);
        new FieldIndexAssert(Item.class, "name").assertSingleEntry(fileSession(), childIdAfterUpdate);
	}
    
    public static void main(String[] arguments) {
        new CsCascadedDeleteReaddChildReferenceTestCase().runAll();
    }


}
