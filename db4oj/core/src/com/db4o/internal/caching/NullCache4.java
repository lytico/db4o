/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.caching;

import java.util.*;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public class NullCache4 <K,V> implements Cache4<K, V>{

	public V produce(K key, Function4<K, V> producer, Procedure4<V> onDiscard) {
		return producer.apply(key);
	}

	public Iterator<V> iterator() {
		return Iterators.platformIterator(Iterators.EMPTY_ITERATOR);
	}
	
}
