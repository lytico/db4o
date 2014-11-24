/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta.instrumentation.test;

import java.io.*;
import java.net.*;


public class ClassFiles {

	public static File fileForClass(Class clazz) throws IOException {
		URL url = clazz.getResource(simpleName(clazz) + ".class");
		return new File(url.getFile());
	}

	private static String simpleName(Class clazz) {
		String clazzName = clazz.getName();
		int dotIdx = clazzName.lastIndexOf('.');
		return clazzName.substring(dotIdx + 1);
	}

	public static String classNameAsPath(Class clazz) {
		return clazz.getName().replace('.', '/') + ".class";
	}

	static byte[] classBytes(Class klass) throws IOException {
		return IO.readAllBytes(fileForClass(klass));
	}

}
