/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class HashtableBase {
	
	private static final float FILL = 0.5F;
	
	// FIELDS ARE PUBLIC SO THEY CAN BE REFLECTED ON IN JDKs <= 1.1

	public int _tableSize;

	public int _mask;

	public int _maximumSize;

	public int _size;

	public HashtableIntEntry[] _table;

	public HashtableBase(int size) {
		size = newSize(size); // legacy for .NET conversion
		_tableSize = 1;
		while (_tableSize < size) {
			_tableSize = _tableSize << 1;
		}
		_mask = _tableSize - 1;
		_maximumSize = (int) (_tableSize * FILL);
		_table = new HashtableIntEntry[_tableSize];
	}

	public HashtableBase() {
		this(1);
	}
	
    /** @param cloneOnlyCtor */
	protected HashtableBase(DeepClone cloneOnlyCtor) {
	}
	
	public void clear() {
		_size = 0;
		Arrays4.fill(_table, null);
	}
	
	private final int newSize(int size) {
		return (int) (size / FILL);
	}

	public int size() {
		return _size;
	}
	
	protected HashtableIntEntry findWithSameKey(HashtableIntEntry newEntry) {
		HashtableIntEntry existing = _table[entryIndex(newEntry)];
		while (null != existing) {
			if (existing.sameKeyAs(newEntry)) {
				return existing;
			}
			existing = existing._next;
		}
		return null;
	}


	protected int entryIndex(HashtableIntEntry entry) {
		return entry._key & _mask;
	}
	
	protected void putEntry(HashtableIntEntry newEntry) {
		HashtableIntEntry existing = findWithSameKey(newEntry);
		if (null != existing) {
			replace(existing, newEntry);
		} else {
			insert(newEntry);
		}
	}
	
	private void insert(HashtableIntEntry newEntry) {
		_size++;
		if (_size > _maximumSize) {
			increaseSize();
		}
		int index = entryIndex(newEntry);
		newEntry._next = _table[index];
		_table[index] = newEntry;
	}
	
	private void replace(HashtableIntEntry existing, HashtableIntEntry newEntry) {
		newEntry._next = existing._next;
		HashtableIntEntry entry = _table[entryIndex(existing)];
		if (entry == existing) {
			_table[entryIndex(existing)] = newEntry;
		} else {
			while (entry._next != existing) {
				entry = entry._next;
			}
			entry._next = newEntry;
		}
	}

	private void increaseSize() {
		_tableSize = _tableSize << 1;
		_maximumSize = _maximumSize << 1;
		_mask = _tableSize - 1;
		HashtableIntEntry[] temp = _table;
		_table = new HashtableIntEntry[_tableSize];
		for (int i = 0; i < temp.length; i++) {
			reposition(temp[i]);
		}
	}

	protected HashtableIterator hashtableIterator() {
		return new HashtableIterator(_table);
	}
	
	private void reposition(HashtableIntEntry entry) {
        HashtableIntEntry currentEntry = entry; 
        HashtableIntEntry nextEntry = null; 
        while (currentEntry != null) 
        { 
            nextEntry = currentEntry._next; 
            currentEntry._next = _table[entryIndex(currentEntry)]; 
            _table[entryIndex(currentEntry)] = currentEntry; 
            currentEntry = nextEntry; 
        } 
	}
	
	public Iterator4 keys() {
		return Iterators.map(hashtableIterator(), new Function4() {
			public Object apply(Object current) {
				return ((Entry4)current).key();
			}
		});
	}
	
	public Iterable4 values() {
		return new Iterable4() {
			public Iterator4 iterator() {
				return valuesIterator();
			}
		};
	}

	/**
	 * Iterates through all the values.
	 * 
	 * @return value iterator
	 */
	public Iterator4 valuesIterator() {
		return Iterators.map(hashtableIterator(), new Function4() {
			public Object apply(Object current) {
				return ((Entry4)current).value();
			}
		});
	}


	public String toString() {
		return Iterators.join(hashtableIterator(), "{", "}", ", ");
	}

	protected void removeEntry(HashtableIntEntry predecessor, HashtableIntEntry entry) {
		if (predecessor != null) {
			predecessor._next = entry._next;
		} else {
			_table[entryIndex(entry)] = entry._next;
		}
		_size--;
	}

	protected Object removeObjectEntry(int intKey, Object objectKey) {
		HashtableObjectEntry entry = (HashtableObjectEntry) _table[intKey & _mask];
		HashtableObjectEntry predecessor = null;
		while (entry != null) {
			if (entry._key == intKey && entry.hasKey(objectKey)) {
				removeEntry(predecessor, entry);
				return entry._object;
			}
			predecessor = entry;
			entry = (HashtableObjectEntry) entry._next;
		}
		return null;
	}
	
	protected Object removeLongEntry(int intKey, long longKey) {
		HashtableLongEntry entry = (HashtableLongEntry) _table[intKey & _mask];
		HashtableLongEntry predecessor = null;
		while (entry != null) {
			if (entry._key == intKey && entry._longKey == longKey) {
				removeEntry(predecessor, entry);
				return entry._object;
			}
			predecessor = entry;
			entry = (HashtableLongEntry) entry._next;
		}
		return null;
	}

	protected Object removeIntEntry(int key) {
		HashtableIntEntry entry = _table[key & _mask];
		HashtableIntEntry predecessor = null;
		while (entry != null) {
			if (entry._key == key) {
				removeEntry(predecessor, entry);
				return entry._object;
			}
			predecessor = entry;
			entry = entry._next;
		}
		return null;
	}
}
