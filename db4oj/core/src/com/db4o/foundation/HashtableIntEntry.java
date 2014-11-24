/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class HashtableIntEntry implements Entry4, DeepClone  {

	// FIELDS ARE PUBLIC SO THEY CAN BE REFLECTED ON IN JDKs <= 1.1

	public int _key;

	public Object _object;

	public HashtableIntEntry _next;

	HashtableIntEntry(int key, Object obj) {
		_key = key;
		_object = obj;
	}

	public HashtableIntEntry() {
	}

	public Object key() {
		return new Integer(_key);
	}
	
	public Object value(){
		return _object;
	}

	public Object deepClone(Object obj) {
		return deepCloneInternal(new HashtableIntEntry(), obj);
	}

	public boolean sameKeyAs(HashtableIntEntry other) {
		return _key == other._key;
	}

	protected HashtableIntEntry deepCloneInternal(HashtableIntEntry entry, Object obj) {
		entry._key = _key;
		entry._next = _next;
		if (_object instanceof DeepClone) {
			entry._object = ((DeepClone) _object).deepClone(obj);
		} else {
			entry._object = _object;
		}
		if (_next != null) {
			entry._next = (HashtableIntEntry) _next.deepClone(obj);
		}
		return entry;
	}
	
	public String toString() {
		return "" + _key + ": " + _object;
	}
}
