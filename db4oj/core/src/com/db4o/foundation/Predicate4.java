/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public interface Predicate4<T> {
	public boolean match(T candidate);
}
