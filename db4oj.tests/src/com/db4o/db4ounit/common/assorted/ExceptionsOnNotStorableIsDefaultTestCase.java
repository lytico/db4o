/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class ExceptionsOnNotStorableIsDefaultTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new ExceptionsOnNotStorableIsDefaultTestCase().runSolo();
    }
    
    protected void configure(Configuration config) throws Exception {
        config.callConstructors(true);
    }
    
    public static class Item {
        
        public Item(Object obj){
            if(obj == null){
                throw new RuntimeException();
            }
        }
        
        public static Item newItem(){
            return new Item(new Object());
        }
    }
    
    public void testObjectContainerAliveAfterObjectNotStorableException(){
        Assert.expect(ObjectNotStorableException.class,new CodeBlock() {
            public void run() throws Throwable {
                store(Item.newItem());
            }
        });
    }
}
