/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * Interface to configure the cache configurations.
 */
public interface CacheConfiguration {
	
    /**
     * configures the size of the slot cache to hold a number of
     * slots in the cache.
     * @param size the number of slots
     * @sharpen.property
     * @deprecated since 7.14 BTrees have their own LRU cache now.
     */
    public void slotCacheSize(int size);

}
