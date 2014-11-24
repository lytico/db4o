/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation.io;

import java.io.*;

/**
 * Keep the API compatible with System.IO.Directory
 * 
 * @sharpen.ignore
 * @sharpen.rename System.IO.Directory
 */
public class Directory4 {
	
	public static void delete(String path, boolean recurse) {
		File f = new File(path);
		if (recurse) {
			final File[] files = f.listFiles();
			if (files != null) {
				delete(files);
			}
		}
		File4.translateDeleteFailureToException(f);
	}

	private static void delete(File[] files) {
		for (int i = 0; i < files.length; i++) {
			File f = files[i];
			if (f.isDirectory()) {
				delete(f.listFiles());
			}
			File4.translateDeleteFailureToException(f);
		}
	}
}
