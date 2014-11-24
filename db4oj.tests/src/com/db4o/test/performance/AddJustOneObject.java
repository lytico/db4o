/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.performance;

import java.io.*;

import com.db4o.*;


public class AddJustOneObject {
    
    
    private static final String FILE = "ajoob.db4o";
    
    private static final int COUNT = 100000;
    

    public static void main(String[] args) {
        new File(FILE).delete();
        ObjectContainer oc = Db4o.openFile(FILE);
        for (int i = 0; i < COUNT; i++) {
            oc.store(new AddJustOneObject());
        }
        oc.close();
        
        oc = Db4o.openFile(FILE);
        long start = System.currentTimeMillis();
        oc.store(new AddJustOneObject());
        oc.commit();
        long stop = System.currentTimeMillis();
        oc.close();
        
        long duration = stop - start;
        
        System.out.println("Add one to " + COUNT + " and commit: " + duration + "ms");
    }

}
