/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.caching;

import java.util.*;

import com.db4o.foundation.*;

/**
 * @exclude
 * Full version of the algorithm taken from here:
 * http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.34.2641
 */
class LRU2QXCache<K,V> implements Cache4<K,V> {

	private final Map<K,V> _slots;	
	private final CircularBuffer4<K> _am; // 'eden': long-term lru queue
	private final CircularBuffer4<K> _a1in; // 'nursery': short-term fifo queue, entry point for all new items
	private final CircularBuffer4<K> _a1out; // 'backlog': fifo queue, elements may not be backed in _slots or may overlap with _am
	private final int _maxSize; // invariant: |_slots| = |_am| + |_a1in| <= _maxSize
	private final int _inSize;
	
	public LRU2QXCache(int maxSize) {
		_maxSize = maxSize;
		_inSize = _maxSize / 4;
		_slots = new HashMap<K,V>(_maxSize);
		_am = new CircularBuffer4<K>(_maxSize);
		_a1in = new CircularBuffer4<K>(_maxSize);
		_a1out = new CircularBuffer4<K>(_maxSize / 2);
	}
	
	public V produce(K key, Function4<K,V> producer, Procedure4<V> finalizer) {
		if(key == null){
			throw new ArgumentNullException();
		}
		if(_am.remove(key)) {
			_am.addFirst(key);
			return _slots.get(key);
		}
		if(_a1out.contains(key)) {
			reclaimFor(key, producer, finalizer);
			_am.addFirst(key);
			return _slots.get(key);
		}
		if(_a1in.contains(key)) {
			return _slots.get(key);
		}
		reclaimFor(key, producer, finalizer);
		_a1in.addFirst(key);
		return _slots.get(key);
	}
	
	private void reclaimFor(K key, Function4<K,V> producer, Procedure4<V> finalizer) {
		if(_slots.size() < _maxSize) {
			_slots.put(key, producer.apply(key));
			return;
		}

		if(_a1in.size() > _inSize) {
			K lastKey = _a1in.removeLast();
			discard(lastKey, finalizer);
			if(_a1out.isFull()) {
				_a1out.removeLast();
			}
			_a1out.addFirst(lastKey);
		} else {
			K lastKey = _am.removeLast();
			discard(lastKey, finalizer);
		}
		_slots.put(key, producer.apply(key));
	}
	
	public Iterator iterator() {
		return _slots.values().iterator();
	}

	public String toString() {
		return "LRU2QXCache(am=" + toString(_am)  + ", a1in=" + toString(_a1in)  + ", a1out=" + toString(_a1out) + ")" + " - " + _slots.size();
	}

	private void discard(K key, Procedure4<V> finalizer) {
		V removed = _slots.remove(key);
		if(finalizer != null) {
			finalizer.apply(removed);
		}
	}

	private String toString(Iterable4<K> buffer) {
		return Iterators.toString(buffer);
	}
}
