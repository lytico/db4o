/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.config;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.foundation.io.*;
import com.db4o.internal.*;
import com.db4o.internal.config.*;

import db4ounit.*;

public class ConfigurationItemTestCase extends Db4oTestWithTempFile {
	
	static final class ConfigurationItemStub implements ConfigurationItem {

		private InternalObjectContainer _container;
		private Configuration _configuration;

		public void apply(InternalObjectContainer container) {
			Assert.isNotNull(container);
			_container = container;
		}

		public void prepare(Configuration configuration) {
			Assert.isNotNull(configuration);
			_configuration = configuration;
		}
		
		public Configuration preparedConfiguration() {
			return _configuration;
		}
		
		public InternalObjectContainer appliedContainer() {
			return _container;
		}
		
	}

	public void test() {
		EmbeddedConfiguration configuration = newConfiguration();
		
		ConfigurationItemStub item = new ConfigurationItemStub();
		configuration.common().add(item);
		
		Assert.areSame(legacyConfigFor(configuration), item.preparedConfiguration());
		Assert.isNull(item.appliedContainer());
		
		File4.delete(tempFile());
		
		ObjectContainer container = Db4oEmbedded.openFile(configuration, tempFile());
		container.close();
		
		Assert.areSame(container, item.appliedContainer());
	}

	private Configuration legacyConfigFor(EmbeddedConfiguration configuration) {
		EmbeddedConfigurationImpl configImpl = (EmbeddedConfigurationImpl) configuration;
		return configImpl.legacy();
	}
}
