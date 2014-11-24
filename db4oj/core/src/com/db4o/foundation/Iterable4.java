/* Copyright (C) 2004-2006   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @sharpen.ignore
 * @sharpen.rename System.Collections.IEnumerable
 */
public interface Iterable4<T> {
	/**
	 * @sharpen.rename GetEnumerator
	 */
	public Iterator4<T> iterator();
}
