/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.staging;

import com.db4o.db4ounit.jre5.enums.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * COR-465
 * merge into SimpleEnumTestCase when fixed
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class EnumPeekPersistedTestCase extends AbstractDb4oTestCase{
	
    public static final class Item {
        
        public TypeCountEnum a;
        
        public Item(){
        }
        
        public Item(TypeCountEnum a_){
            a = a_;
        }
    }
    
    public void testPeekPersisted() throws Exception{
        Item storedItem = new Item(TypeCountEnum.A);
        store(storedItem);
        db().commit();
        reopen();
        Item retrievedItem = (Item) retrieveOnlyInstance(Item.class);
        Item peekedItem = db().peekPersisted(retrievedItem, Integer.MAX_VALUE, true);
        Assert.areSame(retrievedItem.a, peekedItem.a);
    }
    
}
