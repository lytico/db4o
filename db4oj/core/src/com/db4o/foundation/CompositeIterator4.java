/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

public class CompositeIterator4 implements Iterator4 {

	protected final Iterator4 _iterators;	

	protected Iterator4 _currentIterator;
	
	public CompositeIterator4(Iterator4[] iterators) {
		this(new ArrayIterator4(iterators));
	}

	public CompositeIterator4(Iterator4 iterators) {
		if (null == iterators) {
			throw new ArgumentNullException();
		}
		_iterators = iterators;
	}

	public boolean moveNext() {
		while(_currentIterator == null || !_currentIterator.moveNext()) {
			if (!_iterators.moveNext()) {
				return false;
			}
			_currentIterator = nextIterator(_iterators.current());
		}
		return _currentIterator != null;
	}
	
	public void reset() {
		resetIterators();
		_currentIterator = null;
		_iterators.reset();
	}

	private void resetIterators() {
		_iterators.reset();
		while (_iterators.moveNext()) {
			nextIterator(_iterators.current()).reset();
		}
	}
	
	public Iterator4 currentIterator() {
		return _currentIterator;
	}

	public Object current() {
		return _currentIterator.current();
	}
	
	protected Iterator4 nextIterator(final Object current) {
		return (Iterator4)current;
	}
}