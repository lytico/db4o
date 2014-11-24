/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public interface Sequence4<T> extends Iterable4<T> {
	
	boolean add(T element);
	
	void addAll(Iterable4<T> iterable);
	
	boolean isEmpty();

	T get(int index);
	
	int size();
	
	void clear();
	
	boolean remove(T obj);
	
	boolean contains(T obj);
	
	boolean containsAll(Iterable4<T> iter);
	
	Object[] toArray();
	
	T[] toArray(T[] array);
	
}
