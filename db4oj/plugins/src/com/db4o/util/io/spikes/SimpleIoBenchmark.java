/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.util.io.spikes;

import java.io.File;

import com.db4o.Db4o;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.io.IoAdapter;
import com.db4o.util.io.NIOFileAdapter;
import com.db4o.util.io.win32.Win32IoAdapter;

/**
 * @exclude
 */
public class SimpleIoBenchmark {
	
	private static final String DBFILENAME = "SimpleIoBenchmark.db4o";
	private static final int ITERATIONS = 10000;

	public static void main(String[] args) {
		
		for (int i=0; i<3; ++i) {
			System.out.println("*******************");
			test("Default IO adapter", null);
			test("NIOFileadapter", new NIOFileAdapter(1024*32, 16));
			test("Win32IoAdapter", new Win32IoAdapter());
		}
		
	}
	
	private static void test(String name, IoAdapter adapter) {
		if (null != adapter) {
			Db4o.configure().io(adapter);
		}
		
		long start = System.currentTimeMillis();
		store();
		query();
		long elapsed = System.currentTimeMillis() - start;
		
		// System.gc is necessary to circumvent
		//      http://bugs.sun.com/bugdatabase/view_bug.do;:YfiG?bug_id=4724038
		System.gc();

		
		System.out.println(name);
		System.out.println("\t" + elapsed + "ms");
		File file = new File(DBFILENAME);
		System.out.println("\tFile size is " + file.length() + " bytes.");
		System.out.println("\t" + file.length()/elapsed + " bytes/ms");
		if (!file.delete()) {
			System.err.println("Unable to delete '" + DBFILENAME + "'");
		}
	}
	

	private static void query() {
		ObjectContainer db = Db4o.openFile(DBFILENAME);
		try {
			ObjectSet set = db.queryByExample(TestDummy.class);
			if (ITERATIONS != set.size()) {
				System.err.println("Expected: " + ITERATIONS + ", actual: " + set.size());
			}
		} finally {
			db.close();
		}
	}

	private static void store() {
		
		ObjectContainer db = Db4o.openFile(DBFILENAME);
		try {
			for (int i=0; i<ITERATIONS; ++i) {
				db.store(new TestDummy("Dummy " + i));
				if (0 == i % 10) {
					db.commit();
				}
			}
		} finally {
			db.close();
		}
	}
}