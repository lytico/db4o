/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta.instrumentation.test;

import java.io.*;

import com.db4o.instrumentation.util.*;

/**
 * Aids in writing jar files (zip files that contains java classes and resources).
 */
public class JarFileWriter extends ZipFileWriter {

	public JarFileWriter(File file) throws IOException {
		super(file);
	}
	
	public void writeClass(Class klass) throws IOException {
		writeEntry(ClassFiles.classNameAsPath(klass), ClassFiles.classBytes(klass));
	}
}
