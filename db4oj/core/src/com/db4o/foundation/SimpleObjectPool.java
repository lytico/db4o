/* Copyright (C) 2004 - 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

public class SimpleObjectPool<T> implements ObjectPool<T> {
	
	private final Object[] _objects;
	private int _available;

	public SimpleObjectPool(T... objects) {
		final int length = objects.length;
		_objects = new Object[length];
		for (int i=0; i<length; ++i) {
	        _objects[length-i-1] = objects[i];
        }
		_available = length;
    }

	@SuppressWarnings("unchecked")
    public T borrowObject() {
		if (_available == 0) {
			throw new IllegalStateException();
		}
		return (T)_objects[--_available];
	}

	public void returnObject(T o) {
		if (_available == _objects.length) {
			throw new IllegalStateException();
		}
		_objects[_available++] = o;
	}
}
