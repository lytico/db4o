/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.cs.config;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.io.*;

import db4ounit.*;

public class ServerConfigurationItemIntegrationTestCase implements TestCase {

	public void test() {
		ServerConfiguration config = Db4oClientServer.newServerConfiguration();
		config.file().storage(new MemoryStorage());
		DummyConfigurationItem item = new DummyConfigurationItem();
		config.addConfigurationItem(item);
		ObjectServer server = Db4oClientServer.openServer(config, "", Db4oClientServer.ARBITRARY_PORT);
		item.verify(config, server);
		server.close();
	}

	private final class DummyConfigurationItem implements ServerConfigurationItem {
		private int _prepareCount = 0;
		private int _applyCount = 0;
		private ServerConfiguration _config;
		private ObjectServer _server;
		
		public void prepare(ServerConfiguration configuration) {
			_config = configuration;
			_prepareCount++;
		}

		public void apply(ObjectServer server) {
			_server = server;
			_applyCount++;
		}
		
		void verify(ServerConfiguration config, ObjectServer server) {
			Assert.areSame(config, _config);
			Assert.areSame(server, _server);
			Assert.areEqual(1, _prepareCount);
			Assert.areEqual(1, _applyCount);
		}
	}

}
