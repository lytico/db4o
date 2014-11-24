/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.cs.config;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.ext.*;
import com.db4o.io.*;

import db4ounit.*;

public class ClientConfigurationItemIntegrationTestCase implements TestCase {

	private static final String PASSWORD = "db4o";
	private static final String USER = "db4o";

	public void test() {
		ServerConfiguration serverConfig = Db4oClientServer.newServerConfiguration();
		serverConfig.file().storage(new MemoryStorage());
		ObjectServer server = Db4oClientServer.openServer(serverConfig, "", Db4oClientServer.ARBITRARY_PORT);
		server.grantAccess(USER, PASSWORD);
		
		ClientConfiguration clientConfig = Db4oClientServer.newClientConfiguration();
		DummyConfigurationItem item = new DummyConfigurationItem();
		clientConfig.addConfigurationItem(item);
		ExtClient client = (ExtClient) Db4oClientServer.openClient(clientConfig, "localhost", server.ext().port(), USER, PASSWORD);		
		item.verify(clientConfig, client);		
		client.close();
		
		server.close();
	}

	private final class DummyConfigurationItem implements ClientConfigurationItem {
		private int _prepareCount = 0;
		private int _applyCount = 0;
		private ClientConfiguration _config;
		private ExtClient _client;
		
		public void prepare(ClientConfiguration configuration) {
			_config = configuration;
			_prepareCount++;
		}

		public void apply(ExtClient client) {
			_client = client;
			_applyCount++;
		}
		
		void verify(ClientConfiguration config, ExtClient client) {
			Assert.areSame(config, _config);
			Assert.areSame(client, _client);
			Assert.areEqual(1, _prepareCount);
			Assert.areEqual(1, _applyCount);
		}
	}

}
