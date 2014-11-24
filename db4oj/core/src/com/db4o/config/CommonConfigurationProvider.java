package com.db4o.config;


/**
 * A configuration provider that provides access to
 * the common configuration methods that can be called
 * for embedded, server and client use of db4o.
 * @since 7.5
 */
public interface CommonConfigurationProvider {
	
	/**
	 * Access to the common configuration methods.
	 * @sharpen.property
	 */
	CommonConfiguration common();

}
