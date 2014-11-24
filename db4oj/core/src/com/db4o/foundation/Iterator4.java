/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @sharpen.ignore
 * @sharpen.rename System.Collections.IEnumerator
 */
public interface Iterator4<E> {

	public boolean moveNext();

	/**
	 * @sharpen.property
	 */
	public E current();

	public void reset();
}
