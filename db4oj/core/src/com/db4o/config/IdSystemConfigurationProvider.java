/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * A configuration provider that provides access
 * to the IdSystem-related configuration methods.
 */
public interface IdSystemConfigurationProvider {
	
	/**
	 * Access to the IdSystem-related configuration methods.
	 * @sharpen.property
	 */
	public IdSystemConfiguration idSystem();

}
