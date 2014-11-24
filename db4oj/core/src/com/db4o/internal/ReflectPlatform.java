/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import java.lang.reflect.*;

import com.db4o.ext.*;


/**
 * @sharpen.ignore
 */
public class ReflectPlatform {
	
	public static final char INNER_CLASS_SEPARATOR = '$';


	/**
	 * @param className
	 * @return the Class object for specified className. Returns null if an
	 *         error is encountered during loading the class.
	 */
	public static Class forName(String className) {
		try {
			return Class.forName(className);
		} catch (Exception e) {
			// e.printStackTrace();
		} catch (LinkageError e) {
			// e.printStackTrace();
		}
		return null;
	}

	public static Object createInstance(String className) {
		Class clazz = forName(className);
		return createInstance(clazz);
	}

	public static Object createInstance(Class clazz) {
		if (clazz == null) {
			return null;
		}
		try {
			return clazz.newInstance();
		} catch (Throwable t) {
//			throw new RuntimeException(t);
		}
		return null;
	}

	public static String fullyQualifiedName(Class clazz) {
		return clazz.getName();
	}

	public static boolean isNamedClass(Class clazz) {
		return !clazz.isPrimitive();
	}

	@decaf.ReplaceFirst(value="return simpleNameForJdksPriorTo5(clazz);", platforms={decaf.Platform.JDK11, decaf.Platform.JDK12})
	public static String simpleName(Class clazz) {
		return clazz.getSimpleName();
    }
	
	@SuppressWarnings("unused")
    private static String simpleNameForJdksPriorTo5(Class clazz) {
		final String name = clazz.getName();
		final int lastDot = name.lastIndexOf('.');
		return lastDot < 0
			? name
			: name.substring(lastDot + 1);
	}
	
	
	public static <T> T newInstance(Constructor<T> ctor, Object... args) throws Db4oException {
		try {
			return ctor.newInstance(args);
		} catch (IllegalArgumentException e) {
			throw new Db4oException(e);
		} catch (InstantiationException e) {
			throw new Db4oException(e);
		} catch (IllegalAccessException e) {
			throw new Db4oException(e);
		} catch (InvocationTargetException e) {
			throw new Db4oException(e);
		}
	}

	public static String getJavaInterfaceSimpleName(Class clazz) {
		return ReflectPlatform.simpleName(clazz);
	}
	
	public static String containerName(Class clazz) {
		return clazz.getPackage().getName();
	}
	
	public static String adjustClassName(String className, Class clazz) {
		return className;
	}
	
}
