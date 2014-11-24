/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.instrumentation.core;

import com.db4o.instrumentation.api.*;

/**
 * @exclude
 */
public class ClassLoaderNativeClassFactory implements NativeClassFactory {

	private ClassLoader _loader;
	
	public ClassLoaderNativeClassFactory(ClassLoader loader) {
		_loader = loader;
	}

	public Class forName(String className) throws ClassNotFoundException {
		return _loader.loadClass(className);
	}

}
