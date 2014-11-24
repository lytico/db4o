/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.main;

import java.io.*;

import com.db4o.foundation.io.*;
import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.util.*;

/**
 * Enhances classes stored in jar files keeping the jar structure
 * untouched.
 */
public class Db4oJarEnhancer {
	
	private final Db4oFileInstrumentor _fileEnhancer;

	public Db4oJarEnhancer(BloatClassEdit classEdit) {
		_fileEnhancer = new Db4oFileInstrumentor(classEdit);
	}

	public void enhance(File inputJar, File outputJar, String[] classPath) throws Exception {
		final File workingDir = tempDir(inputJar.getName());
		try {
			extractJarTo(inputJar, workingDir);
			enhance(workingDir, classPath);
			makeJarFromDirectory(workingDir, outputJar);
		} finally {
			deleteDirectory(workingDir);
		}
	}

	private void deleteDirectory(File workingDir) {
		Directory4.delete(workingDir.getAbsolutePath(), true);
	}

	private void enhance(File workingDir, String[] classPath) throws Exception  {
		_fileEnhancer.enhance(workingDir, workingDir, classPath);
	}

	private File tempDir(String name) {
		return new File(Path4.combine(Path4.getTempPath(), name + "-working"));
	}

	private void extractJarTo(File inputJar, File workingDir) throws IOException {
		new ZipFileExtraction(inputJar, workingDir);
	}
	
	private void makeJarFromDirectory(File workingDir, File outputJar) throws IOException {
		new ZipFileCreation(workingDir, outputJar);
	}
}
