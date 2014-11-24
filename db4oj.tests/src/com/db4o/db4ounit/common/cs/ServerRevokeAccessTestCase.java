/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.internal.config.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ServerRevokeAccessTestCase
	extends Db4oClientServerTestCase
	implements OptOutAllButNetworkingCS {

	private static final String SERVER_HOSTNAME = "127.0.0.1";

	public static void main(String[] args) {
		new ServerRevokeAccessTestCase().runAll();
	}

	/**
	 * @sharpen.if !CF
	 */
	public void test() throws IOException {
		final String user = "hohohi";
		final String password = "hohoho";
		ObjectServer server = clientServerFixture().server();
		server.grantAccess(user, password);

		ObjectContainer con = openClient(user, password);
		Assert.isNotNull(con);
		con.close();

		server.ext().revokeAccess(user);

		Assert.expect(Exception.class, new CodeBlock() {
			public void run() throws Throwable {
				openClient(user, password);
			}
		});
	}

	private ObjectContainer openClient(final String user, final String password) {
		return com.db4o.cs.Db4oClientServer.openClient(Db4oClientServerLegacyConfigurationBridge.asClientConfiguration(config()), SERVER_HOSTNAME,
				clientServerFixture().serverPort(), user, password);
	}

	private Configuration config() {
	    return clientServerFixture().config();
    }
}
