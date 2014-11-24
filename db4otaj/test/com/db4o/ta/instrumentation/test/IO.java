/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta.instrumentation.test;

import java.io.*;

import com.db4o.db4ounit.util.*;
import com.db4o.foundation.io.*;

import db4ounit.extensions.util.*;


public class IO {

	public static String mkTempDir(String path) {
		final String tempDir = IOServices.buildTempPath(path);
		File4.mkdirs(tempDir);
		return tempDir;
	}

	public static byte[] readAllBytes(File file) throws IOException {
		return File4.readAllBytes(file.getAbsolutePath());
	}
}
