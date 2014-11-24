/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class Stack4 {
	
	private List4 _tail;

	public void push(Object obj) {
		_tail = new List4(_tail, obj);
	}

	public Object peek() {
		if(_tail == null){
			return null;
		}
		return _tail._element;
	}
	
	public Object pop() {
		if(_tail == null){
			throw new IllegalStateException();
		}
		Object res = _tail._element;
		_tail = _tail._next;
		return res;
	}

	public boolean isEmpty() {
		return _tail==null;
	}
	
}
