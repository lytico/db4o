/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.connection.test;

import static com.db4o.omplus.test.util.Db4oTestUtil.*;
import static org.junit.Assert.*;

import java.io.*;
import java.util.*;

import org.junit.*;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.omplus.connection.*;

public class RemoteConnectionParamsTestCase {

	private static final String HOST = "localhost";
	private static final String USER = "db4o_user";
	private static final String PASSWORD = "db4o_pass";

	@Test
	public void testNoServer() {
		RemoteConnectionParams params = new RemoteConnectionParams(HOST, 0xdb40, USER, PASSWORD);
		try {
			params.connect();
			fail();
		} 
		catch (DBConnectException exc) {
		}
	}

	@Test
	public void testWrongCredentials() throws Exception {
		File dbFile = createEmptyDatabase();
		ObjectServer server = openServer(dbFile);
		RemoteConnectionParams params = new RemoteConnectionParams(HOST, server.ext().port(), USER, PASSWORD + "X");
		try {
			params.connect();
			fail();
		} 
		catch (DBConnectException exc) {
		}
		finally {
			server.close();
			dbFile.delete();
		}
	}

	@Test
	public void testOpen() throws Exception {
		File dbFile = createEmptyDatabase();
		ObjectServer server = openServer(dbFile);
		RemoteConnectionParams params = new RemoteConnectionParams(HOST, server.ext().port(), USER, PASSWORD);
		try {
			ObjectContainer client = params.connect();
			assertNotNull(client);
			client.close();
		}
		finally {
			server.close();
			dbFile.delete();
		}
	}

	@Test
	public void testEquals() {
		String[] hosts = { "foo.invalid", "bar.invalid" };
		int[] ports = { 42, 43 };
		String[] users = { "Ann", "Bob" };
		String[] passwords = { "**", "***" };
		String[][] jarPaths = { {}, { "baz.jar" } };
		String[] configNames = { "BazConfig" };
		List<List<RemoteConnectionParams>> params = new ArrayList<List<RemoteConnectionParams>>();
		List<List<RemoteConnectionParams>> clones = new ArrayList<List<RemoteConnectionParams>>();
		for (String host : hosts) {
			for (int port : ports) {
				List<RemoteConnectionParams> curParams = new ArrayList<RemoteConnectionParams>();
				List<RemoteConnectionParams> curClones = new ArrayList<RemoteConnectionParams>();
				for (String user : users) {
					for (String password : passwords) {
						for (String[] jarPath : jarPaths) {
							curParams.add(new RemoteConnectionParams(host, port, user, password, Arrays.copyOf(jarPath, jarPath.length), new String[0]));
							curClones.add(new RemoteConnectionParams(host, port, user, password, Arrays.copyOf(jarPath, jarPath.length), new String[0]));
							if(jarPath.length > 0) {
								curParams.add(new RemoteConnectionParams(host, port, user, password, Arrays.copyOf(jarPath, jarPath.length), Arrays.copyOf(configNames, configNames.length)));
								curClones.add(new RemoteConnectionParams(host, port, user, password, Arrays.copyOf(jarPath, jarPath.length), Arrays.copyOf(configNames, configNames.length)));
							}
						}
					}
				}
				params.add(curParams);
				clones.add(curClones);
			}
		}
		for (int outer = 0; outer < params.size(); outer++) {
			assertEquals(clones.get(outer), params.get(outer));
			assertEquals(clones.get(outer).hashCode(), params.get(outer).hashCode());
			for (int inner = 0; inner < params.size(); inner++) {
				if(outer == inner) {
					continue;
				}
				assertFalse(params.get(outer).equals(params.get(inner)));
			}				
		}
	}

	private ObjectServer openServer(File dbFile) {
		ServerConfiguration serverConfig = Db4oClientServer.newServerConfiguration();
		ObjectServer server = Db4oClientServer.openServer(serverConfig, dbFile.getAbsolutePath(), -1);
		server.grantAccess(USER, PASSWORD);
		return server;
	}

}
