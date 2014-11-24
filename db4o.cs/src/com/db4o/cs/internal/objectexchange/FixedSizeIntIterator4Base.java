/**
 * 
 */
package com.db4o.cs.internal.objectexchange;

import com.db4o.foundation.*;

public abstract class FixedSizeIntIterator4Base implements FixedSizeIntIterator4 {
    private final int _size;
    private int _current;
    private int _available;

    public FixedSizeIntIterator4Base(int size) {
	    this._size = size;
	    _available = size;
    }

    public int size() {
    	return _size;
    }

    public int currentInt() {
    	return _current;
    }

    public Object current() {
    	return _current;
    }

    public boolean moveNext() {
    	if (_available > 0) {
    		--_available;
    		_current = nextInt();
    		return true;
    	}
    	return false;
    }
    
    protected abstract int nextInt();

    public void reset() {
        throw new com.db4o.foundation.NotImplementedException();
    }
}