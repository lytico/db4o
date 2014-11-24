/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.config;

import com.db4o.config.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class CacheConfigurationImpl implements CacheConfiguration{
	
	private final Config4Impl _config;

	public CacheConfigurationImpl(Config4Impl config) {
		_config = config;
	}

	/**
	 * @sharpen.property
	 * @deprecated since 7.14 BTrees have their own LRU cache now.
	 */
	public void slotCacheSize(int size) {
		
	}
	
}
