package com.db4o.db4ounit.common.reflect.custom;

import com.db4o.*;
import com.db4o.foundation.*;

public class ObjectSetIterator implements Iterator4 {

	private final ObjectSet _set;
	private Object _current;

	public ObjectSetIterator(ObjectSet set) {
		_set = set;
	}

	public Object current() {
		return _current;
	}

	public boolean moveNext() {
		if (_set.hasNext()) {
			_current = _set.next();
			return true;
		}
		return false;
	}

	public void reset() {
		throw new NotImplementedException();
	}

}
