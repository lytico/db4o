/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.migration;

import java.io.*;
import java.lang.reflect.*;
import java.net.*;

import com.db4o.db4ounit.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Db4oLibraryEnvironment {
	
	private final static String[] PREFIXES = { "com.db4o" };
	private final ClassLoader _loader;
	
	public Db4oLibraryEnvironment(File db4oLibrary, File additionalClassPath) throws IOException {
		_loader = new VersionClassLoader(urls(db4oLibrary, additionalClassPath), PREFIXES);
	}

	private URL[] urls(File db4oLibrary, File additionalClassPath)
			throws MalformedURLException {
		return new URL[] { toURL(db4oLibrary), toURL(additionalClassPath) };
	}

	/**
	 * @deprecated using deprecated api
	 */
	private URL toURL(File db4oLibrary) throws MalformedURLException {
		return db4oLibrary.toURL();
	}
	
	public String version() throws Exception {
		String version = (String)invokeStaticMethod("com.db4o.Db4o", "version");
		return version.substring(5);
	}	

	private Object invokeStaticMethod(String className, String methodName) throws Exception {
        Class clazz = _loader.loadClass(className);
        Method method = clazz.getMethod(methodName, new Class[] {});
        return method.invoke(null, new Object[] {});
	}
	
	public Object invokeInstanceMethod(Class klass, String methodName, Object... args) throws Exception {
		Class clazz = _loader.loadClass(klass.getName());
        Method method = clazz.getMethod(methodName, classes(args));
        return method.invoke(clazz.newInstance(), args);
	}

	private Class[] classes(Object[] args) {
		Class[] classes = new Class[args.length];
		for (int i=0; i<args.length; ++i) {
			classes[i] = args[i].getClass();
		}
		return classes;
	}

	public void dispose() {
		// do nothing on the Java side
		
	}	
}
