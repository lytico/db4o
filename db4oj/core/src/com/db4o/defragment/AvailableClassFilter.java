/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import com.db4o.ext.*;

/**
 * Filter that accepts only StoredClass instances whose corresponding Java
 * class is currently known.
 * @sharpen.ignore
 */
public class AvailableClassFilter implements StoredClassFilter {
	
	private ClassLoader _loader;

	/**
	 * Will accept only classes that are known to the classloader that loaded
	 * this class.
	 */
	public AvailableClassFilter() {
		this(AvailableClassFilter.class.getClassLoader());
	}

	/**
	 * Will accept only classes that are known to the given classloader.
	 * 
	 * @param loader The classloader to check class names against
	 */
	public AvailableClassFilter(ClassLoader loader) {
		_loader = loader;
	}

	/**
	 * Will accept only classes whose corresponding platform class is known
	 * to the configured classloader.
	 * 
	 * @param storedClass The class instance to be checked
	 * @return true if the corresponding platform class is known to the configured classloader, false otherwise
	 */
	public boolean accept(StoredClass storedClass) {
		try {
			_loader.loadClass(storedClass.getName());
			return true;
		} catch (ClassNotFoundException exc) {
			return false;
		}
	}
}
