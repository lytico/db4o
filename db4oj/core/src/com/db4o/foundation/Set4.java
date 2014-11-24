/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

public interface Set4<T> extends Iterable4<T> {
	boolean add(T obj);
	void clear();
	boolean contains(T obj);
	boolean isEmpty();
	Iterator4<T> iterator();
	boolean remove(T obj);
	int size();
}
