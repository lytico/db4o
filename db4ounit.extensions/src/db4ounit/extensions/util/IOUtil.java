/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.util;

import java.io.*;

import com.db4o.foundation.io.*;

/**
 * @exclude
 */
public class IOUtil {

	/**
	 * Deletes the directory
	 */
	public static void deleteDir(String dir) throws IOException {
		String absolutePath = new File(dir).getCanonicalPath();
		File fDir = new File(dir);
		if (fDir.isDirectory()) {
			String[] files = fDir.list();
			for (int i = 0; i < files.length; i++) {
				deleteDir(Path4.combine(absolutePath, files[i]));
			}
		}
		File4.delete(dir);
	}
}
