/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * A configuration provider that provides access
 * to the cache-related configuration methods.
 */
public interface CacheConfigurationProvider {
	
	/**
	 * Access to the cache-related configuration methods.
	 * @sharpen.property
	 */
	CacheConfiguration cache();

}
