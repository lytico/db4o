/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.fixtures;

import com.db4o.foundation.*;


public class ContextfulIterator extends Contextful implements Iterator4 {

	private final Iterator4 _delegate;
	
	public ContextfulIterator(Iterator4 delegate) {
		_delegate = delegate;
	}

	public Object current() {
		return run(new Closure4() {
			public Object run() {
				return _delegate.current();
			}
		});
	}

	public boolean moveNext() {
		final BooleanByRef result = new BooleanByRef();
		run(new Runnable() {
			public void run() {
				result.value = _delegate.moveNext();
			}
		});
		return result.value;
	}

	public void reset() {
		run(new Runnable() {
			public void run() {
				_delegate.reset();
			}
		});
	}

}
