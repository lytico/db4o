/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

public class ArrayIterator4 extends IndexedIterator {
	
	private final Object[] _elements;
	
	public ArrayIterator4(Object[] elements) {
		super(elements.length);
		_elements = elements;
	}

	protected Object get(final int index) {
		return _elements[index];
	}	
}
