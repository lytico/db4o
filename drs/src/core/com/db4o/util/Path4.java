package com.db4o.util;
/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * This file was taken from the db4oj.tests project from
 * the com.db4o.db4ounit.util package. 
 * TODO: move to own project and supply as separate Jar.
 */

import java.io.*;

import com.db4o.foundation.*;


public class Path4 { 
	
	private static final java.util.Random _random = new java.util.Random();
	
	public static String getDirectoryName(String targetPath) {
		return new File(targetPath).getParent();
	}

	public static String combine(String parent, String child) {		
		return parent.endsWith(java.io.File.separator)
        ? parent + child
        : parent + java.io.File.separator + child;
	}
	
	public static String getTempPath() {
		String path = System.getProperty("java.io.tmpdir"); 
		if(path == null || path.length() <= 1){
		    path = "/temp"; 
		}
		File4.mkdirs(path);
		return path;
	}

	public static String getTempFileName() {
		String tempPath = getTempPath();
		while (true) {
			String fname = combine(tempPath, "db4o-test-" + nextRandom() + ".tmp");
			if (!File4.exists(fname)) {
				try {
					new FileWriter(fname).close();
				} catch (IOException e) {
					throw new RuntimeException(e.getMessage());
				}
				return fname;
			}
		}
	}

	private static String nextRandom() {
		return Integer.toHexString(_random.nextInt());
	}

}
