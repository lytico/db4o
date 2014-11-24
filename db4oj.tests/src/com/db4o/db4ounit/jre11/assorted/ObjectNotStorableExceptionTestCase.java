/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.assorted;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class ObjectNotStorableExceptionTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new ObjectNotStorableExceptionTestCase().runSolo();
    }
    
    protected void configure(Configuration config) throws Exception {
        config.callConstructors(true);
        config.exceptionsOnNotStorable(true);
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
        assertNotStorableException(Item.newItem());
    }
    
    public void testPrimitiveCantBeStored() {
    	assertNotStorableException(42);
    }
    
    public void testStringCantBeStored() {
    	assertNotStorableException("42");
    }
    
    private void assertNotStorableException(final Object object) {
	    Assert.expect(ObjectNotStorableException.class,new CodeBlock() {
            public void run() throws Throwable {
                store(object);
            }
        });
        
        Assert.expect(ObjectNotStorableException.class,new CodeBlock() {
            public void run() throws Throwable {
                store(object);
            }
        });
    }
    

}
