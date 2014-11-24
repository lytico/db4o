/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class Collection4Iterator extends Iterator4Impl {

	private final Collection4 _collection;
	private final int _initialVersion;

	public Collection4Iterator(Collection4 collection, List4 first) {
		super(first);
		_collection = collection;
		_initialVersion = currentVersion();
	}
	
	public boolean moveNext() {
		validate();
		return super.moveNext();
	}
	
	public Object current() {
		validate();
		return super.current();
	}

	private void validate() {
		if (_initialVersion != currentVersion()) {
			// FIXME: change to ConcurrentModificationException
			throw new InvalidIteratorException();
		}
	}
	
	private int currentVersion() {
		return _collection.version();
	}
}
