/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class IntIterator4Impl implements FixedSizeIntIterator4 { 
	
	private final int _count;
	private int[] _content;
	private int _current;
	
	public IntIterator4Impl(int[] content, int count) {
		_content = content;
		_count = count;
		reset();
	}

	public int currentInt() {
		if (_content == null || _current == _count) {
			throw new IllegalStateException();
		}
		return _content[_current];
	}

	public Object current() {
		return new Integer(currentInt());
	}

	public boolean moveNext() {
		if (_current < _count - 1) {
			_current ++;
			return true;
		}
		_content = null;
		return false;
	}
	
	public void reset() {
		_current = -1;
	}

	public int size() {
		return _count;
    }

}
