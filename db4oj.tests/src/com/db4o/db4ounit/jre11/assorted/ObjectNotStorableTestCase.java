package com.db4o.db4ounit.jre11.assorted;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ObjectNotStorableTestCase extends AbstractDb4oTestCase {
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.exceptionsOnNotStorable(false);
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
    
    public void testIsNotStored(){
        
        assertIsNotStored(Item.newItem());
    }

	private void assertIsNotStored(final Item item) {
	    store(item);
        Assert.isFalse(db().isStored(item));
    }

}
