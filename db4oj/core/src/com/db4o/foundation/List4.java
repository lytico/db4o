/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

import com.db4o.types.*;

/**
 * simplest possible linked list
 * 
 * @exclude
 */
public final class List4<T> implements Unversioned
{
	// TODO: encapsulate field access
	/**
	 * next element in list
	 */
	public List4<T> _next;
	
	/**
	 * carried object
	 */
	public T _element;  
	
	/**
	 * db4o constructor to be able to store objects of this class
	 */
	public List4() {}
	
	public List4(T element) {
		_element = element;
	}

	public List4(List4<T> next, T element) {
		_next = next;
		_element = element;
	}

	boolean holds(T obj) {
		if(obj == null){
			return _element == null;
		}
		return obj.equals(_element);
	}

	public static int size(List4<?> list) {
		int counter = 0;
		List4 nextList = list; 
		while(nextList != null){
			counter++;
			nextList = nextList._next;
		}
		return counter;
	}
	
}
