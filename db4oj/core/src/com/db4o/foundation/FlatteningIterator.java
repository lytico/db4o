/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

public class FlatteningIterator extends CompositeIterator4 {
	
	private static class IteratorStack {
		public final Iterator4 iterator;
		public final IteratorStack next;
		
		public IteratorStack(Iterator4 iterator_, IteratorStack next_) {
			iterator = iterator_;
			next = next_;
		}
	}
	
	private IteratorStack _stack;

	public FlatteningIterator(Iterator4 iterators) {
		super(iterators);
	}

	public boolean moveNext() {
		if (null == _currentIterator) {
			if (null == _stack) {
				_currentIterator = _iterators;
			} else {
				_currentIterator = pop();
			}
		}
		if (!_currentIterator.moveNext()) {
			if (_currentIterator == _iterators) {
				return false;
			}
			_currentIterator = null;
			return moveNext();
		}
		
		final Object current = _currentIterator.current();
		if (current instanceof Iterator4) {
			push(_currentIterator);
			_currentIterator = nextIterator(current);
			return moveNext();
		}
		return true;
	}

	private void push(Iterator4 currentIterator) {
		_stack = new IteratorStack(currentIterator, _stack);
	}

	private Iterator4 pop() {
		final Iterator4 iterator = _stack.iterator;
		_stack = _stack.next;
		return iterator;
	}

}
