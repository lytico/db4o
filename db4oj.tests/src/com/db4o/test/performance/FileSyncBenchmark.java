/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.test.performance;

import java.io.*;


public class FileSyncBenchmark {

    private static String FILE = "FileSyncBenchmark.file";
    
    private static final int COUNT = 1000;
    
    private static final int SIZE = 1000;

    public static void main(String[] args) throws IOException {
        new File(FILE).delete();
        RandomAccessFile raf = new RandomAccessFile(FILE, "rw");
        for (int i = 0; i < SIZE; i++) {
            raf.write(1);
        }
        raf.close();
        raf = new RandomAccessFile(FILE, "rw");
        long start = System.currentTimeMillis();
        for (int i = 0; i < COUNT; i++) {
        	raf.write(1);
            raf.getFD().sync();
        }
        long stop = System.currentTimeMillis();
        long duration = stop - start;
        raf.close();
        new File(FILE).delete();
        System.out.println("Time for " + COUNT + " syncs of a " + SIZE + " bytes sized file:\n" + duration + "ms");
    }
}
