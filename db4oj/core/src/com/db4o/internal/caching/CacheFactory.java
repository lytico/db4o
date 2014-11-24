/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.caching;

/**
 * @exclude
 */
public class CacheFactory {

	public static <K, V> Cache4<K, V> new2QCache(int size) {
		return new LRU2QCache<K, V>(size);
	}
	
	public static <V> Cache4<Long, V> new2QLongCache(int size) {
		return new LRU2QLongCache<V>(size);
	}

	public static <K, V> Cache4<K, V> new2QXCache(int size) {
		return new LRU2QXCache<K, V>(size);
	}

	public static <K, V> PurgeableCache4<K, V> newLRUCache(int size) {
		return new LRUCache<K, V>(size);
	}
	
	public static <V> PurgeableCache4<Integer, V> newLRUIntCache(int size) {
		return new LRUIntCache<V>(size);
	}
	
	public static <V> PurgeableCache4<Long, V> newLRULongCache(int size) {
		return new LRULongCache<V>(size);
	}


}
