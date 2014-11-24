/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */
/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.api;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;

import db4ounit.*;

public class Db4oClientServerTestCase extends TestWithTempFile {
	
	public void testClientServerApi() {
		final ServerConfiguration config = Db4oClientServer.newServerConfiguration();
		
		final ObjectServer server = Db4oClientServer.openServer(config, tempFile(), 0xdb40);
		try {
			server.grantAccess("user", "password");
			
			final ClientConfiguration clientConfig = Db4oClientServer.newClientConfiguration();
			final ObjectContainer client1 = Db4oClientServer.openClient(clientConfig, "localhost", 0xdb40, "user", "password");
			try {
				
			} finally {
				Assert.isTrue(client1.close());
			}
		} finally {
			Assert.isTrue(server.close());
		}
	}
	
	public void testConfigurationHierarchy() {
		Assert.isInstanceOf(NetworkingConfigurationProvider.class, Db4oClientServer.newClientConfiguration());
		Assert.isInstanceOf(NetworkingConfigurationProvider.class, Db4oClientServer.newServerConfiguration());
	}
	
	
}
