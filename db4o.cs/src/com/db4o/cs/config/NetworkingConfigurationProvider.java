/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.cs.config;


/**
 * A configuration provider that provides access to the
 * networking configuration methods.
 * @since 7.5
 */
public interface NetworkingConfigurationProvider {

	/**
	 * Access to the networking configuration methods.
	 * @sharpen.property
	 */
	NetworkingConfiguration networking();
}
