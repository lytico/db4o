/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class HashtableObjectEntry extends HashtableIntEntry {

	// FIELDS ARE PUBLIC SO THEY CAN BE REFLECTED ON IN JDKs <= 1.1

	public Object _objectKey;

	HashtableObjectEntry(int a_hash, Object a_key, Object a_object) {
		super(a_hash, a_object);
		_objectKey = a_key;
	}

	HashtableObjectEntry(Object a_key, Object a_object) {
		super(a_key.hashCode(), a_object);
		_objectKey = a_key;
	}
	
	public HashtableObjectEntry() {
		super();
	}
	
	@Override
	public Object key(){
		return _objectKey;
	}

	@Override
	public Object deepClone(Object obj) {
        return deepCloneInternal(new HashtableObjectEntry(), obj);
	}
    
	@Override
	protected HashtableIntEntry deepCloneInternal(HashtableIntEntry entry, Object obj) {
        ((HashtableObjectEntry)entry)._objectKey = _objectKey;
        return super.deepCloneInternal(entry, obj);
    }

	public boolean hasKey(Object key) {
		return _objectKey.equals(key);
	}

	@Override
	public boolean sameKeyAs(HashtableIntEntry other) {
		return other instanceof HashtableObjectEntry
			? hasKey(((HashtableObjectEntry) other)._objectKey)
			: false;
	}
	
	@Override
	public String toString() {
		return "" + _objectKey + ": " + _object;
	}
}
