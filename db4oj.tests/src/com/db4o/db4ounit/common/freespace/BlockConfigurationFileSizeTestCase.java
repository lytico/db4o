/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.freespace;

import com.db4o.config.*;

import db4ounit.*;


public class BlockConfigurationFileSizeTestCase extends FileSizeTestCaseBase{

    public static void main(String[] args) {
        new BlockConfigurationFileSizeTestCase().runSolo();
    }
    
    public static class Item {
        
        public String _name;
        
        public Item(String name){
            _name = name;
        }
    }
    
    protected void configure(Configuration config) throws Exception {
        config.blockSize(8);
    }
    
    public void test(){
        store(new Item("one"));
        db().commit();
        int initialSize = databaseFileSize();
        for (int i = 0; i < 100; i++) {
            store(new Item("two"));
        }
        db().commit();
        int modifiedSize = databaseFileSize();
        int sizeIncrease = modifiedSize - initialSize;
        Assert.isSmaller(30000, sizeIncrease);
    }

}
