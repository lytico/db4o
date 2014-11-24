/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.defragment;

import com.db4o.ext.*;

/**
 * Filter for StoredClass instances.
 */
public interface StoredClassFilter {
	/**
	 * @param storedClass StoredClass instance to be checked
	 * @return true, if the given StoredClass instance should be accepted, false otherwise.
	 */
	boolean accept(StoredClass storedClass);
}
