/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta.instrumentation.test;

import java.io.*;
import java.net.*;

import db4ounit.*;
import db4ounit.extensions.util.*;

/**
 * Creates a separate environment to load classes ({@link ExcludingClassLoader}
 * so they can be asserted after instrumentation.
 * 
 * @sharpen.ignore
 */
public class AssertingClassLoader {

	private final URLClassLoader _loader;

	public AssertingClassLoader(File classPath, Class[] excludedClasses) throws MalformedURLException {
		this(classPath, excludedClasses, new Class[0]);
	}

	public AssertingClassLoader(File classPath, Class[] excludedClasses, Class[] delegatedClasses) throws MalformedURLException {
		ExcludingClassLoader excludingLoader = new ExcludingClassLoader(getClass().getClassLoader(), excludedClasses, delegatedClasses);		
		_loader = new URLClassLoader(new URL[] { toURL(classPath) }, excludingLoader);
	}

	/**
	 * @deprecated
	 */
	private URL toURL(File classPath) throws MalformedURLException {
		return classPath.toURL();
	}

	public void assertAssignableFrom(Class expected, Class actual) throws ClassNotFoundException {
		if (isAssignableFrom(expected, actual)) {
			return;
		}
		
		fail(expected, actual, "not assignable from");
	}

	public void assertNotAssignableFrom(Class expected, Class actual) throws ClassNotFoundException {
		if (!isAssignableFrom(expected, actual)) {
			return;
		}
		
		fail(expected, actual, "assignable from");
	}
	
	private void fail(Class expected, Class actual, String reason) {
		Assert.fail("'" + actual + "' " + reason + " '" + expected + "'");
	}

	private boolean isAssignableFrom(Class expected, Class actual) throws ClassNotFoundException {
		Class loaded = loadClass(actual);
		return expected.isAssignableFrom(loaded);
	}

	public Class loadClass(Class actual) throws ClassNotFoundException {
		return _loader.loadClass(actual.getName());
	}

	public Object newInstance(Class clazz) throws InstantiationException, IllegalAccessException, ClassNotFoundException {
		return loadClass(clazz).newInstance();
	}
}
