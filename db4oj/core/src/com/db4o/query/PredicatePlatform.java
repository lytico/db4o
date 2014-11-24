/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.query;

import java.lang.reflect.*;

/**
 * @exclude
 * @sharpen.ignore
 */
public final class PredicatePlatform {
	
	/**
	 * public for implementation reasons, please ignore.
	 */
	public final static String PREDICATEMETHOD_NAME="match";

	public static boolean isFilterMethod(Method method) {
		if (method.getParameterTypes().length != 1) {
			return false;
		}
		return method.getName().equals(PredicatePlatform.PREDICATEMETHOD_NAME);
	}
}
