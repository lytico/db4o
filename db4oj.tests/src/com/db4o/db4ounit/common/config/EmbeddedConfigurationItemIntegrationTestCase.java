/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.config;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.io.*;

import db4ounit.*;

public class EmbeddedConfigurationItemIntegrationTestCase implements TestCase {

	public void test() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.file().storage(new MemoryStorage());
		DummyConfigurationItem item = new DummyConfigurationItem();
		config.addConfigurationItem(item);
		EmbeddedObjectContainer container = Db4oEmbedded.openFile(config, "");		
		item.verify(config, container);		
		container.close();
	}

	private final class DummyConfigurationItem implements EmbeddedConfigurationItem {
		private int _prepareCount = 0;
		private int _applyCount = 0;
		private EmbeddedConfiguration _config;
		private EmbeddedObjectContainer _container;
		
		public void prepare(EmbeddedConfiguration configuration) {
			_config = configuration;
			_prepareCount++;
		}

		public void apply(EmbeddedObjectContainer container) {
			_container = container;
			_applyCount++;
		}
		
		void verify(EmbeddedConfiguration config, EmbeddedObjectContainer container) {
			Assert.areSame(config, _config);
			Assert.areSame(container, _container);
			Assert.areEqual(1, _prepareCount);
			Assert.areEqual(1, _applyCount);
		}
	}

}
