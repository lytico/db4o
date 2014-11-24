/**
 * 
 */
package com.db4o.foundation;

class HashtableByteArrayEntry extends HashtableObjectEntry {

	public HashtableByteArrayEntry(byte[] bytes, Object value) {
		super(hash(bytes), bytes, value);
	}

	public boolean hasKey(Object key) {
		if (key instanceof byte[]) {
			return areEqual((byte[]) i_objectKey, (byte[]) key);
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