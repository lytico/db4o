package com.db4o.db4ounit.common.fieldindex;


import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class StringFieldIndexDefragmentTestCase extends AbstractDb4oTestCase implements OptOutDefragSolo {
	
    public static class Item {
    	
        public String _name;

		public Item(String name) {
			_name = name;
		}
        
		@Override
		public String toString() {
			return _name;
		}
    }

    // We need at least 700 items so IDs overlap with addresses. 
    private final static int ITEM_COUNT = 1000;
    
    @Override
    protected void configure(Configuration config) throws Exception {
    	config.objectClass(Item.class).objectField("_name").indexed(true);
    }
    

    public void test() throws Exception {
    	defragment();
    	for (int i = 0; i < ITEM_COUNT; i++){
    		Query query = newQuery(Item.class);
    		query.descend("_name").constrain(String.valueOf(i));
    		ObjectSet<Item> result = query.execute();
    		Assert.areEqual(1, result.size());
    	}
	}

	@Override
	protected void store() {
        for (int i = 0; i < ITEM_COUNT; i++){
            store(new Item(String.valueOf(i)));
        }
	}

}
