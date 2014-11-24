/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.freespace;


import db4ounit.*;


public class FileSizeTestCase extends FreespaceManagerTestCaseBase {
    
    private static final int ITERATIONS = 100;

	public static void main(String[] args) {
		new FileSizeTestCase().runSolo();
	}
	
	public void testConsistentSizeOnDefragment(){
		storeSomeItems();
		db().commit();
        assertConsistentSize(new Runnable() {
            public void run() {
            	try {
					defragment();
				} catch (Exception e) {
					e.printStackTrace();
				}
            }
        });
	}
	
	public void testConsistentSizeOnRollback(){
		storeSomeItems();
		produceSomeFreeSpace();
        assertConsistentSize(new Runnable() {
            public void run() {
                store(new Item());
                db().rollback();
            }
        });
	}
    
    public void testConsistentSizeOnCommit(){
        storeSomeItems();
        db().commit();
        assertConsistentSize(new Runnable() {
            public void run() {
                db().commit();
            }
        });
    }
    
    public void testConsistentSizeOnUpdate(){
        storeSomeItems();
        produceSomeFreeSpace();
        final Item item = new Item(); 
        store(item);
        db().commit();
        assertConsistentSize(new Runnable() {
            public void run() {
                store(item);
                db().commit();
            }
        });
    }
    
    public void testConsistentSizeOnReopen() throws Exception{
        db().commit();
        reopen();
        assertConsistentSize(new Runnable() {
            public void run() {
                try {
                    reopen();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }
    
    public void testConsistentSizeOnUpdateAndReopen() throws Exception{
        produceSomeFreeSpace();
        store(new Item());
        db().commit();
        assertConsistentSize(new Runnable() {
            public void run() {
                store(retrieveOnlyInstance(Item.class));
                db().commit();
                try {
                    reopen();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }
    
    public void assertConsistentSize(Runnable runnable){
        warmup(runnable);
        int originalFileSize = databaseFileSize();
        for (int i = 0; i < ITERATIONS; i++) {
//        	System.out.println(databaseFileSize());
            runnable.run();
        }
        Assert.areEqual(originalFileSize, databaseFileSize());
    }

	private void warmup(Runnable runnable) {
		for (int i = 0; i < 10; i++) {
//        	System.out.println(databaseFileSize());
            runnable.run();
        }
	}

}
