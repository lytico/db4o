/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.osgi;

import org.osgi.framework.*;

import com.db4o.reflect.jdk.*;

public class OSGiLoader implements JdkLoader {
	
	private final Bundle _bundle;
	private JdkLoader _loader;
	
	public OSGiLoader(Bundle bundle, JdkLoader loader) {
		_bundle = bundle;
		_loader = loader;
	}

	public Class loadClass(String className) {
		Class clazz = _loader.loadClass(className);
		if (clazz != null) {
			return clazz;
		}
		try {
			return _bundle.loadClass(className);
		} 
		catch (ClassNotFoundException exc) {
			return null;
		}
	}

	public Object deepClone(Object context) {
		return new OSGiLoader(_bundle, (JdkLoader) _loader.deepClone(context));
	}

}
