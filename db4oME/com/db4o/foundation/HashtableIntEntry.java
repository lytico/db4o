/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * 
 */
class HashtableIntEntry implements DeepClone {

	int i_key;

	Object i_object;

	HashtableIntEntry i_next;

	HashtableIntEntry(int a_hash, Object a_object) {
		i_key = a_hash;
		i_object = a_object;
	}

	protected HashtableIntEntry() {
	}

	public void acceptKeyVisitor(Visitor4 visitor) {
		visitor.visit(new Integer(i_key));
	}

	public Object deepClone(Object obj) {
		return deepCloneInternal(new HashtableIntEntry(), obj);
	}

	public boolean sameKeyAs(HashtableIntEntry other) {
		return i_key == other.i_key;
	}

	protected HashtableIntEntry deepCloneInternal(HashtableIntEntry entry, Object obj) {
		entry.i_key = i_key;
		entry.i_next = i_next;
		if (i_object instanceof DeepClone) {
			entry.i_object = ((DeepClone) i_object).deepClone(obj);
		} else {
			entry.i_object = i_object;
		}
		if (i_next != null) {
			entry.i_next = (HashtableIntEntry) i_next.deepClone(obj);
		}
		return entry;
	}
}
