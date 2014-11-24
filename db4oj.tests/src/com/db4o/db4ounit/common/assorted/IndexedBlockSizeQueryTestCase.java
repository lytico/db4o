/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class IndexedBlockSizeQueryTestCase extends AbstractDb4oTestCase{

    public static void main(String[] args) {
        new IndexedBlockSizeQueryTestCase().runNetworking();
    }
    
    protected void configure(Configuration config) throws Exception {
        config.blockSize(10);
        config.objectClass(Item.class).objectField("_name").indexed(true);
    }
    
    public static class Item {
        
        // public Object _untypedMember;
        
        public String _name;
        
        public Item(String name){
            // _untypedMember = name;
            _name = name;
        }
    }
    
    protected void store() throws Exception {
        store(new Item("one"));
    }
    
    public void test(){
        Query q = newQuery(Item.class);
        q.descend("_name").constrain("one");
        Assert.areEqual(1, q.execute().size());
    }

}
