/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.performance;

import java.io.*;

import com.db4o.*;


public class MeasureInsertPerformanceScalability {
    
    public static class Item{
        
        public int value;
        
        public Item() {
            
        }
        
        public Item(int value_) {
            value = value_;
        }
        
    }
    
    private static final String FILE = "mips.db4o";
    
    private static final int TOTAL_COUNT = 500000;
    
    private static final int BULK_COUNT = 5000;
    

    public static void main(String[] args) {
        new MeasureInsertPerformanceScalability().run();
    }

    private void run() {
    	
    	long start = System.currentTimeMillis();
        prepare();
        ObjectContainer objectContainer = Db4o.openFile(FILE);
        
        boolean bulk = false;
        int count = 0;
        while(count < TOTAL_COUNT){
            if(bulk){
                count += storeBulk(objectContainer);
            }else{
                count += storeSingle(objectContainer);
            }
            System.out.println("Objects: " + count);
            bulk = ! bulk;
        }
        objectContainer.close();
        
        long stop = System.currentTimeMillis();
        long duration = stop - start;
        
        System.out.println("Total test duration " + duration + "ms");
    }

    private int storeSingle(ObjectContainer objectContainer) {
        long start = System.currentTimeMillis();
        objectContainer.store(new Item((int)start));
        objectContainer.commit();
        long stop = System.currentTimeMillis();
        long duration = stop - start;
        System.out.println("Single " + duration + "ms");
        return 1;
    }

    private int storeBulk(ObjectContainer objectContainer) {
        long start = System.currentTimeMillis();
        for (int i = 0; i < BULK_COUNT; i++) {
            objectContainer.store(new Item((int)start));
        }
        objectContainer.commit();
        long stop = System.currentTimeMillis();
        long duration = stop - start;
        System.out.println("Bulk " + duration + "ms");
        return BULK_COUNT;
    }

    private void prepare() {
        Db4o.configure().objectClass(Item.class).objectField("value").indexed(true);
        new File(FILE).delete();
    }
    
}
