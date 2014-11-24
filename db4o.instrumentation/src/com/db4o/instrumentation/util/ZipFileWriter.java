/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.util;

import java.io.*;
import java.util.zip.*;

/**
 * Simpler facade on top of the {@link ZipOutputStream} api.
 */
public class ZipFileWriter {

	private final ZipOutputStream _zipWriter;

	public ZipFileWriter(File file) throws IOException {
		_zipWriter = new ZipOutputStream(new FileOutputStream(file));
	}

	public void writeResourceString(String fileName, String contents) throws IOException {
		writeEntry(fileName, contents.getBytes());
	}	
	
	public void close() throws IOException {
		_zipWriter.close();
	}
	
	public void writeEntry(String entryName, final byte[] bytes)	throws IOException {
		beginEntry(entryName, bytes.length);
		try {
			_zipWriter.write(bytes);
		} finally {
			endEntry();
		}
	}
	
	private void beginEntry(final String entryName, int length) throws IOException {
		final ZipEntry entry = new ZipEntry(entryName);
		entry.setSize(length);
		_zipWriter.putNextEntry(entry);
	}

	private void endEntry() throws IOException {
		_zipWriter.closeEntry();
	}
}
