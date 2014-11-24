/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.foundation;

class HashtableByteArrayEntry extends HashtableObjectEntry {

	public HashtableByteArrayEntry(byte[] bytes, Object value) {
		super(hash(bytes), bytes, value);
	}

    public HashtableByteArrayEntry(){
        super();
    }
    
    public Object deepClone(Object obj) {
        return deepCloneInternal(new HashtableByteArrayEntry(), obj);
    }

	public boolean hasKey(Object key) {
		if (key instanceof byte[]) {
			return areEqual((byte[]) key(), (byte[]) key);
		}
		return false;
	}

	static int hash(byte[] bytes) {
		int ret = 0;
		for (int i = 0; i < bytes.length; i++) {
			ret = ret * 31 + bytes[i];
		}
		return ret;
	}

	static boolean areEqual(byte[] lhs, byte[] rhs) {
		if (rhs.length != lhs.length) return false;
		for (int i = 0; i < rhs.length; i++) {
			if (rhs[i] != lhs[i]) return false;
		}
		return true;
	}
}