/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class IntIterator4Adaptor implements IntIterator4{
	
	private final Iterator4 _iterator;

	public IntIterator4Adaptor(Iterator4 iterator) {
		_iterator = iterator;
	}
	
	public IntIterator4Adaptor(Iterable4 iterable){
		this(iterable.iterator());
	}

	public int currentInt() {
		return ((Integer)current()).intValue();
	}

	public Object current() {
		return _iterator.current();
	}

	public boolean moveNext() {
		return _iterator.moveNext();
	}

	public void reset() {
		_iterator.reset();
	}
	
}
