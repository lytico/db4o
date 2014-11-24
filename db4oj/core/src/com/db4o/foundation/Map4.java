/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public interface Map4 {

	int size();
	
	Object get(Object key);

	void put(Object key, Object value);
	
	boolean containsKey(Object key);

	Object remove(Object key);

	Iterable4 values();
	
	void clear();

}