/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.enums;

import db4ounit.*;
import db4ounit.extensions.*;



/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class SimpleEnumTestCase extends AbstractDb4oTestCase {
    
    public static void main(String[] arguments) {
        new SimpleEnumTestCase().runEmbedded();
    }
    
    public static final class Item {
        
        public TypeCountEnum a;
        
        public Item(){
        }
        
        public Item(TypeCountEnum a_){
            a = a_;
        }
    }
    
    public void testRetrieve() throws Exception{
        Item storedItem = new Item(TypeCountEnum.A);
        store(storedItem);
        db().commit();
        reopen();
        Item retrievedItem = (Item) retrieveOnlyInstance(Item.class);
        Assert.areSame(retrievedItem.a, TypeCountEnum.A);
    }
    
}
