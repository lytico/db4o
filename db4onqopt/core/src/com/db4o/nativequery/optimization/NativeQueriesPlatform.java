/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.nativequery.optimization;

import java.lang.reflect.*;

/**
 * @sharpen.ignore
 */
public class NativeQueriesPlatform {
	
	public static String toPlatformName(String javaName) {
		return javaName;
	}

	public static boolean isStatic(Method method) {
		return Modifier.isStatic(method.getModifiers());
	}

}
