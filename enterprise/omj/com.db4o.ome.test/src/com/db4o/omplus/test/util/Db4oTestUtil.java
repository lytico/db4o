/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.test.util;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;

public class Db4oTestUtil {

	private static final String PREFIX = "ometest";
	private static final String SUFFIX = ".db4o";

	public static File createEmptyDatabase() throws IOException {
		File file = nonExistentFile();
		EmbeddedObjectContainer db = Db4oEmbedded.openFile(config(), file.getAbsolutePath());
		db.close();
		return file;
	}

	public static File nonExistentFile() throws IOException {
		File file = File.createTempFile(PREFIX, SUFFIX);
		file.delete();
		return file;
	}

	private static EmbeddedConfiguration config() {
		return Db4oEmbedded.newConfiguration();
	}
	
	private Db4oTestUtil() {
	}
	
}
