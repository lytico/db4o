/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.caching;

import java.util.*;

import com.db4o.foundation.*;

/**
 * @exclude
 */
class LRUCache<K, V> implements PurgeableCache4<K, V> {
	
	private final Map<K, V> _slots;
	private final CircularBuffer4<K> _lru;
	private final int _maxSize;

	LRUCache(int size) {
		_maxSize = size;
		_slots = new HashMap<K, V>(size);
		_lru = new CircularBuffer4<K>(size);
	}

	public V produce(K key, Function4<K, V> producer, Procedure4<V> finalizer) {
		final V value = _slots.get(key);
		if (value == null) {
			final V newValue = producer.apply(key);
			if (newValue == null) {
				return null;
			}
			if (_slots.size() >= _maxSize) {
				final V discarded = _slots.remove(_lru.removeLast());
				if (null != finalizer) {
					finalizer.apply(discarded);
				}
			}
			_slots.put(key, newValue);
			_lru.addFirst(key);
			return newValue;
		}
		
		_lru.remove(key); // O(N) 
		_lru.addFirst(key);
		return value;
	}

	public Iterator iterator() {
		return _slots.values().iterator();
	}

	public V purge(K key) {
		V removed = _slots.remove(key);
		if(removed == null){
			return null;
		}
		_lru.remove(key);
		return removed;
    }
}

