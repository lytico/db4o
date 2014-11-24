/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.util;

import java.io.*;

import com.db4o.foundation.io.*;

/**
 * Creates a zip from the contents of a directory.
 * 
 * The operation is performed as a side effect of the
 * constructor execution.
 */
public class ZipFileCreation {

	private final ZipFileWriter _zipFile;
	private final File _baseDir;

	public ZipFileCreation(File sourceDir, File outputFile) throws IOException {
		_baseDir = sourceDir.getCanonicalFile();
		_zipFile = new ZipFileWriter(outputFile);
		try {
			writeEntries(_baseDir.listFiles());
		} finally {
			_zipFile.close();
		}
	}

	private void writeEntries(File[] files) throws IOException {
		for (int i = 0; i < files.length; i++) {
			writeEntry(files[i]);
		}
	}

	private void writeEntry(File file) throws IOException {
		if (file.isDirectory()) {
			writeEntries(file.listFiles());
			return;
		}
		writeFileEntry(file);
	}

	private void writeFileEntry(File file) throws IOException {
		_zipFile.writeEntry(relativePath(file).replace('\\', '/'), readAllBytes(file));
	}

	private byte[] readAllBytes(File file) throws IOException {
		return File4.readAllBytes(file.getAbsolutePath());
	}
	
	private String relativePath(File file) throws IOException {
		final String basePath = _baseDir.getAbsolutePath();
		final String filePath = file.getCanonicalPath();
		assertPathPrefix(basePath, filePath);
		return filePath.substring(basePath.length() + 1);
	}

	private void assertPathPrefix(final String expectedPrefix,
			final String actualPath) {
		if (!actualPath.startsWith(expectedPrefix)) {
			// how come?
			throw new IllegalStateException();
		}
	}
}
