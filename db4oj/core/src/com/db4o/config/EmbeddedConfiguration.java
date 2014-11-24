package com.db4o.config;

/**
 * Configuration interface for db4o in embedded use.
 * @since 7.5
 */
public interface EmbeddedConfiguration extends FileConfigurationProvider, CommonConfigurationProvider, CacheConfigurationProvider, IdSystemConfigurationProvider {

	/**
     * adds ConfigurationItems to be applied when
     * a networking {@link EmbeddedObjectContainer} is opened. 
     * @param configItem the {@link EmbeddedConfigurationItem}
     * @since 7.12
     */
	void addConfigurationItem(EmbeddedConfigurationItem configItem);

}
