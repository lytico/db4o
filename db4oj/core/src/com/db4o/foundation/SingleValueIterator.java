/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;


public class SingleValueIterator implements Iterator4 {
	
	private Object _value;
	private boolean _moved;

	public SingleValueIterator(Object value) {
		_value = value;
	}

	public Object current() {
		if (!_moved || _value == Iterators.NO_ELEMENT) {
			throw new IllegalStateException();
		}
		return _value;
	}

	public boolean moveNext() {
		if (!_moved) {
			_moved = true;
			return true;
		}
		_value = Iterators.NO_ELEMENT;
		return false;
	}

	public void reset() {
		throw new NotImplementedException();
	}

}
