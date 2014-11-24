/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation.io;

import java.io.*;

import com.db4o.foundation.*;



/**
 * IMPORTANT: Keep the interface of this class compatible with .NET System.IO.Path otherwise
 * bad things will happen to you.
 * 
 * @sharpen.ignore
 * @sharpen.rename System.IO.Path
 */
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
