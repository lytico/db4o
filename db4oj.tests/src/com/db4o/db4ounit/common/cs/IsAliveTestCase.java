package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.internal.*;
import com.db4o.db4ounit.common.api.*;

import db4ounit.*;

public class IsAliveTestCase extends TestWithTempFile {
	
	private final static String USERNAME = "db4o";
	private final static String PASSWORD = "db4o";
	
	public void testIsAlive() {
		ObjectServer server = openServer();
		int port = server.ext().port();
		ClientObjectContainer client = openClient(port);
		Assert.isTrue(client.isAlive());
		client.close();
		server.close();
	}

	public void testIsNotAlive() {
		ObjectServer server = openServer();
		int port = server.ext().port();
		ClientObjectContainer client = openClient(port);
		server.close();
		Assert.isFalse(client.isAlive());
		client.close();
	}
	
	private ObjectServer openServer() {
		ObjectServer server = Db4oClientServer.openServer(Db4oClientServer.newServerConfiguration(), tempFile(), -1);
		server.grantAccess(USERNAME, PASSWORD);
		return server;
	}

	private ClientObjectContainer openClient(int port) {
		ClientObjectContainer client = (ClientObjectContainer) Db4oClientServer.openClient(Db4oClientServer.newClientConfiguration(), "localhost", port, USERNAME, PASSWORD);
		return client;
	}

}
