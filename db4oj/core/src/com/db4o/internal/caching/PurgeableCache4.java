/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.internal.caching;

/**
 * @exclude
 */
public interface PurgeableCache4<K, V> extends Cache4<K, V> {
	
	/**
	 * Removes the cached value with the specified key from this cache.
	 * 
	 * @param key
	 * @return the purged value or null
	 */
	V purge(K key);

}
