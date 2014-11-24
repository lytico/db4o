/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.caching;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public interface Cache4<K, V> extends Iterable<V> {
	
	/**
	 * Retrieves the value associated to the {@link key} from the cache. If the value is not yet
	 * cached {@link producer} will be called to produce it. If the cache needs to discard a value
	 * {@link finalizer} will be given a chance to process it.
	 * 
	 * @param key the key for the value - must never change - cannot be null
	 * @param producer will be called if value not yet in the cache - can only be null when the value is found in the cache
	 * @param finalizer will be called if a page needs to be discarded - can be null
	 * 
	 * @return the cached value
	 */
	V produce(K key, Function4<K,V> producer, Procedure4<V> finalizer);

}
