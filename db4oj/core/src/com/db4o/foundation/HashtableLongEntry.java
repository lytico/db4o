/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class HashtableLongEntry extends HashtableIntEntry {

	// FIELDS ARE PUBLIC SO THEY CAN BE REFLECTED ON IN JDKs <= 1.1

	public long _longKey;

	HashtableLongEntry(long key, Object obj) {
		super((int)key, obj);
		_longKey = key;
	}
	
	public HashtableLongEntry() {
		super();
	}
	
	@Override
	public Object key(){
		return _longKey;
	}

	@Override
	public Object deepClone(Object obj) {
        return deepCloneInternal(new HashtableLongEntry(), obj);
	}
    
	@Override
	protected HashtableIntEntry deepCloneInternal(HashtableIntEntry entry, Object obj) {
        ((HashtableLongEntry)entry)._longKey = _longKey;
        return super.deepCloneInternal(entry, obj);
    }

	@Override
	public boolean sameKeyAs(HashtableIntEntry other) {
		return other instanceof HashtableLongEntry
			? ((HashtableLongEntry)other)._longKey == _longKey
			: false;
	}
	
	@Override
	public String toString() {
		return "" + _longKey + ": " + _object;
	}
}
