package com.db4o.foundation;

public class ArrayIterator4 implements Iterator4 {
	
	Object[] _elements;
	int _next;

	public ArrayIterator4(Object[] elements) {
		_elements = elements;
		_next = 0;
	}

	public boolean hasNext() {
		return _next < _elements.length;
	}

	public Object next() {
		return _elements[_next++]; 
	}

}
