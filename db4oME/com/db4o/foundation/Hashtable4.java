/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class Hashtable4 implements DeepClone {

	private static final float FILL = 0.5F;

	private int i_tableSize;

	private int i_mask;

	private int i_maximumSize;

	private int i_size;

	private HashtableIntEntry[] i_table;

	public Hashtable4(int a_size) {
		a_size = newSize(a_size); // legacy for .NET conversion
		i_tableSize = 1;
		while (i_tableSize < a_size) {
			i_tableSize = i_tableSize << 1;
		}
		i_mask = i_tableSize - 1;
		i_maximumSize = (int) (i_tableSize * FILL);
		i_table = new HashtableIntEntry[i_tableSize];
	}

	protected Hashtable4() {
	}

	public Object deepClone(Object obj) {
		return deepCloneInternal(new Hashtable4(), obj);
	}

	public void forEachKey(Visitor4 visitor) {
		for (int i = 0; i < i_table.length; i++) {
			HashtableIntEntry entry = i_table[i];
			while (entry != null) {
				entry.acceptKeyVisitor(visitor);
				entry = entry.i_next;
			}
		}
	}

	public void forEachKeyForIdentity(Visitor4 visitor, Object a_identity) {
		for (int i = 0; i < i_table.length; i++) {
			HashtableIntEntry entry = i_table[i];
			while (entry != null) {
				if (entry.i_object == a_identity) {
					entry.acceptKeyVisitor(visitor);
				}
				entry = entry.i_next;
			}
		}
	}

	public void forEachValue(Visitor4 visitor) {
		for (int i = 0; i < i_table.length; i++) {
			HashtableIntEntry entry = i_table[i];
			while (entry != null) {
				visitor.visit(entry.i_object);
				entry = entry.i_next;
			}
		}
	}

	public Object get(byte[] key) {
		int intKey = HashtableByteArrayEntry.hash(key);
		return getFromObjectEntry(intKey, key);
	}

	public Object get(int key) {
		HashtableIntEntry entry = i_table[key & i_mask];
		while (entry != null) {
			if (entry.i_key == key) {
				return entry.i_object;
			}
			entry = entry.i_next;
		}
		return null;
	}

	public Object get(Object key) {
		if (key == null) {
			return null;
		}
		int intKey = key.hashCode();
		return getFromObjectEntry(intKey, key);
	}

	public void put(byte[] key, Object value) {
		putEntry(new HashtableByteArrayEntry(key, value));
	}

	public void put(int key, Object value) {
		putEntry(new HashtableIntEntry(key, value));
	}

	public void put(Object key, Object value) {
		putEntry(new HashtableObjectEntry(key, value));
	}

	public Object remove(byte[] key) {
		int intKey = HashtableByteArrayEntry.hash(key);
		return removeObjectEntry(intKey, key);
	}

	public void remove(int a_key) {
		HashtableIntEntry entry = i_table[a_key & i_mask];
		HashtableIntEntry predecessor = null;
		while (entry != null) {
			if (entry.i_key == a_key) {
				removeEntry(predecessor, entry);
				return;
			}
			predecessor = entry;
			entry = entry.i_next;
		}
	}

	public void remove(Object objectKey) {
		int intKey = objectKey.hashCode();
		removeObjectEntry(intKey, objectKey);
	}

	protected Hashtable4 deepCloneInternal(Hashtable4 ret, Object obj) {
		ret.i_mask = i_mask;
		ret.i_maximumSize = i_maximumSize;
		ret.i_size = i_size;
		ret.i_tableSize = i_tableSize;
		ret.i_table = new HashtableIntEntry[i_tableSize];
		for (int i = 0; i < i_tableSize; i++) {
			if (i_table[i] != null) {
				ret.i_table[i] = (HashtableIntEntry) i_table[i].deepClone(obj);
			}
		}
		return ret;
	}

	private int entryIndex(HashtableIntEntry entry) {
		return entry.i_key & i_mask;
	}

	private HashtableIntEntry findWithSameKey(HashtableIntEntry newEntry) {
		HashtableIntEntry existing = i_table[entryIndex(newEntry)];
		while (null != existing) {
			if (existing.sameKeyAs(newEntry)) {
				return existing;
			}
			existing = existing.i_next;
		}
		return null;
	}

	private Object getFromObjectEntry(int intKey, Object objectKey) {
		HashtableObjectEntry entry = (HashtableObjectEntry) i_table[intKey & i_mask];
		while (entry != null) {
			if (entry.i_key == intKey && entry.hasKey(objectKey)) {
				return entry.i_object;
			}
			entry = (HashtableObjectEntry) entry.i_next;
		}
		return null;
	}

	private void increaseSize() {
		i_tableSize = i_tableSize << 1;
		i_maximumSize = i_maximumSize << 1;
		i_mask = i_tableSize - 1;
		HashtableIntEntry[] temp = i_table;
		i_table = new HashtableIntEntry[i_tableSize];
		for (int i = 0; i < temp.length; i++) {
			reposition(temp[i]);
		}
	}

	private void insert(HashtableIntEntry newEntry) {
		i_size++;
		if (i_size > i_maximumSize) {
			increaseSize();
		}
		int index = entryIndex(newEntry);
		newEntry.i_next = i_table[index];
		i_table[index] = newEntry;
	}

	private final int newSize(int a_size) {
		return (int) (a_size / FILL);
	}

	private void putEntry(HashtableIntEntry newEntry) {
		HashtableIntEntry existing = findWithSameKey(newEntry);
		if (null != existing) {
			replace(existing, newEntry);
		} else {
			insert(newEntry);
		}
	}

	private void removeEntry(HashtableIntEntry predecessor, HashtableIntEntry entry) {
		if (predecessor != null) {
			predecessor.i_next = entry.i_next;
		} else {
			i_table[entryIndex(entry)] = entry.i_next;
		}
		i_size--;
	}

	private Object removeObjectEntry(int intKey, Object objectKey) {
		HashtableObjectEntry entry = (HashtableObjectEntry) i_table[intKey & i_mask];
		HashtableObjectEntry predecessor = null;
		while (entry != null) {
			if (entry.i_key == intKey && entry.hasKey(objectKey)) {
				removeEntry(predecessor, entry);
				return entry.i_object;
			}
			predecessor = entry;
			entry = (HashtableObjectEntry) entry.i_next;
		}
		return null;
	}

	private void replace(HashtableIntEntry existing, HashtableIntEntry newEntry) {
		newEntry.i_next = existing.i_next;
		HashtableIntEntry entry = i_table[entryIndex(existing)];
		if (entry == existing) {
			i_table[entryIndex(existing)] = newEntry;
		} else {
			while (entry.i_next != existing) {
				entry = entry.i_next;
			}
			entry.i_next = newEntry;
		}
	}

	private void reposition(HashtableIntEntry a_entry) {
		if (a_entry != null) {
			reposition(a_entry.i_next);
			a_entry.i_next = i_table[entryIndex(a_entry)];
			i_table[entryIndex(a_entry)] = a_entry;
		}
	}

}
