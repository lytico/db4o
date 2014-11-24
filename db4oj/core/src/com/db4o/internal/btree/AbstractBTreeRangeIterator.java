/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.internal.btree;

import com.db4o.foundation.*;

public abstract class AbstractBTreeRangeIterator implements Iterator4 {

	private final BTreeRangeSingle _range;
	private BTreePointer _cursor;
	private BTreePointer _current;

	public AbstractBTreeRangeIterator(BTreeRangeSingle range) {
		_range = range;
		BTreePointer first = range.first();
		if(first != null){
			// we clone here, because we are calling unsafeNext() on BTreePointer
			// _cursor is our private copy, we modify it and never pass it out.
			_cursor = first.shallowClone();
		}
	}

	public boolean moveNext() {
		if (reachedEnd()) {
			_current = null;
			return false;
		}
		if(_current == null){
			_current = _cursor.shallowClone();
		} else {
			_cursor.copyTo(_current);
		}
		_cursor = _cursor.unsafeFastNext();
		return true;		
	}
	
	public void reset() {
		_cursor = _range.first();
	}

	protected BTreePointer currentPointer() {
		if (null == _current) {
			throw new IllegalStateException();
		}
		return _current;
	}

	private final boolean reachedEnd() {
	    if(_cursor == null){
	        return true;
	    }
	    if(_range.end() == null){
	        return false;
	    }
	    return _range.end().equals(_cursor);
	}
}