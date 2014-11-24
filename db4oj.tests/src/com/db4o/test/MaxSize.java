/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.*;
import com.db4o.query.*;

/**
 * 
 */
public class MaxSize {

    public String           index;

    private static final int SIZE = 1000000;
    private static final int COMMIT_INTERVAL = 10000;
    private static final int QUERIES = 1000;
    private static final String FILE = "maxSize.db4o"; 
    
    public static void main(String[] args) {
        
        // make sure there is an index on the "index" field
        Db4o.configure().objectClass(MaxSize.class).objectField("index").indexed(true);
        
        store();
        query();
    }

    public MaxSize() {

    }

    public MaxSize(String index) {
        this.index = index;
    }

    public static void store() {
        new File(FILE).delete();
        ObjectContainer objectContainer = Db4o.openFile(FILE);
        long start = System.currentTimeMillis();
        long elapsed;
        for (int i = 1; i <= SIZE; i++) {
            objectContainer.store(new MaxSize("" + i));
            if (((double) i / (double) COMMIT_INTERVAL) == i / COMMIT_INTERVAL) {
                objectContainer.commit();
                objectContainer.ext().purge();
                elapsed = System.currentTimeMillis() - start; 
                System.out.println("Committed " + i + " from " + SIZE + " elapsed " + elapsed + "ms");
            }
        }
        objectContainer.close();
        elapsed = System.currentTimeMillis() - start;
        System.out.println("\nTime to store " + SIZE + " objects:\n" + elapsed + "ms");
    }
    
    public static void query() {
        
        ObjectContainer objectContainer = Db4o.openFile(FILE);
        long time = System.currentTimeMillis();
        for (int i = 1; i <= QUERIES; i++) {
            Query q = objectContainer.query();
            q.constrain(MaxSize.class);
            q.descend("index").constrain("" + i);
            q.execute();
        }
        time = System.currentTimeMillis() - time;
        objectContainer.close();
        System.out.println("\nTime for  " + QUERIES + " queries against an indexed field in " + SIZE + " objects:\n"  + time + "ms");
        double perQuery = (double)time / (double)1000;
        System.out.println("\nTime per query:\n"  + perQuery + "ms");
        int perSecond = (int)((double)1000 / perQuery);
        System.out.println("\nQueries per second:\n" + perSecond);
    }
    
    
}