/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

/**
 * Basic functionality for implementing iterators for
 * fixed length structures whose elements can be efficiently
 * accessed by a numeric index.
 */
public abstract class IndexedIterator implements Iterator4 {

	private final int _length;
	private int _next;

	public IndexedIterator(int length) {
		_length = length;
		_next = -1;
	}

	public boolean moveNext() {
		if (_next < lastIndex()) {
			++_next;
			return true;
		}
		// force exception on unexpected call to current
		_next = _length;
		return false;
	}

	public Object current() {
		return get(_next); 
	}
	
	public void reset() {
		_next = -1;
	}
	
	protected abstract Object get(final int index);

	private int lastIndex() {
		return _length - 1;
	}

}