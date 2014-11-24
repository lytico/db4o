/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

public class SynchronizedIterator4  implements Iterator4 {
	
	private final Iterator4 _delegate;
	
	private final Object _lock;
	
	public SynchronizedIterator4(Iterator4 delegate_, Object lock){
		_delegate = delegate_;
		_lock = lock;
	}

	public Object current() {
		synchronized(_lock){
			return _delegate.current();
		}
	}

	public boolean moveNext() {
		synchronized(_lock){
			return _delegate.moveNext();
		}
	}

	public void reset() {
		synchronized(_lock){
			_delegate.reset();
		}
	}

}
