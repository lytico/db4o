/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.instrumentation.core;

import com.db4o.instrumentation.api.*;

/**
 * @exclude
 */
public class DefaultNativeClassFactory implements NativeClassFactory {

	public Class forName(String className) throws ClassNotFoundException {
		return Class.forName(className);
	}

}
