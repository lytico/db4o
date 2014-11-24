/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.io;

import com.db4o.ext.*;
import com.db4o.internal.caching.*;


/**
 * Caching storage adapter to cache db4o database data in memory
 * until the underlying {@link Bin} is instructed to flush its 
 * data when {@link Bin#sync()} is called.<br><br> 
 * 
 * You can override the {@link #newCache()} method if you want to
 * work with a different caching strategy. 
 */
public class CachingStorage extends StorageDecorator {

	private static int DEFAULT_PAGE_COUNT = 64;
	private static int DEFAULT_PAGE_SIZE = 1024;
	
	private int _pageCount;
	private int _pageSize;

	/**
	 * default constructor to create a Caching storage with the default
	 * page count of 64 and the default page size of 1024.
	 * @param storage the {@link Storage} to be cached.
	 */
	public CachingStorage(Storage storage) {
	    this(storage, DEFAULT_PAGE_COUNT, DEFAULT_PAGE_SIZE);
    }

	/**
	 * constructor to set up a CachingStorage with a configured page count
	 * and page size
	 * @param storage the {@link Storage} to be cached.
	 * @param pageCount the number of pages the cache should use.
	 * @param pageSize the size of the pages the cache should use.
	 */
	public CachingStorage(Storage storage, int pageCount, int pageSize) {
	    super(storage);
		_pageCount = pageCount;
		_pageSize = pageSize;
    }

	/**
	 * opens a Bin for the given URI.
	 */
	@Override
	public Bin open(BinConfiguration config) throws Db4oIOException {
	    final Bin storage = super.open(config);
	    if (config.readOnly()) {
	    	return new ReadOnlyBin(new NonFlushingCachingBin(storage, newCache(), _pageCount, _pageSize));
	    }
	    return new CachingBin(storage, newCache(), _pageCount, _pageSize);
	}

	/**
	 * override this method if you want to work with a different caching
	 * strategy than the default LRU2Q cache. 
	 */
	protected Cache4<Long, Object> newCache() {
	    return CacheFactory.newLRULongCache(_pageCount);
    }

	private static final class NonFlushingCachingBin extends CachingBin {
		
		public NonFlushingCachingBin(Bin bin, Cache4 cache, int pageCount, int pageSize) throws Db4oIOException {
			super(bin, cache, pageCount, pageSize);
		}
		
		@Override 
		protected void flushAllPages() {
		}
	}
}
