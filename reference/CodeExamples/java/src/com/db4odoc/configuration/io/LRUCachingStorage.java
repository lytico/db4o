package com.db4odoc.configuration.io;

import com.db4o.internal.caching.Cache4;
import com.db4o.internal.caching.CacheFactory;
import com.db4o.io.CachingStorage;
import com.db4o.io.Storage;

// #example: Exchange the cache-implementation
public class LRUCachingStorage extends CachingStorage {
    private final int pageCount;

    public LRUCachingStorage(Storage storage) {
        super(storage);
        this.pageCount = 128;
    }

    public LRUCachingStorage(Storage storage, int pageCount, int pageSize) {
        super(storage, pageCount, pageSize);
        this.pageCount = pageCount;
    }

    @Override
    protected Cache4<Long, Object> newCache() {
        return CacheFactory.newLRUCache(pageCount);
    }
}
// #end example
