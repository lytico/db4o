package com.db4o.instrumentation.file;

import java.io.*;

/**
 * @exclude
 */
public interface InstrumentationClassSource {
    File sourceFile();
	String className() throws IOException;
	File targetPath(File targetBase) throws IOException;
	InputStream inputStream() throws IOException;
}