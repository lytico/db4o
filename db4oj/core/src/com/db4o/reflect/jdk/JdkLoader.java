/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.jdk;

import com.db4o.foundation.*;

/**
 * @exclude
 * 
 * @sharpen.ignore
 */
public interface JdkLoader extends DeepClone {
	Class loadClass(String className);
}
