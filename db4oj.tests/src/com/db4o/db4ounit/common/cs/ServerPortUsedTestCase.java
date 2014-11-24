/* Copyright (C) 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.cs.*;
import com.db4o.ext.*;
import com.db4o.foundation.io.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ServerPortUsedTestCase extends Db4oClientServerTestCase implements OptOutAllButNetworkingCS{

	private static final String DATABASE_FILE = "PortUsed.db";

	public static void main(String[] args) {
		new ServerPortUsedTestCase().runAll();
	}

	protected void db4oTearDownBeforeClean() throws Exception {
		File4.delete(DATABASE_FILE);

	}

	public void test() {
		final int port = clientServerFixture().serverPort();
		Assert.expect(Db4oIOException.class, new CodeBlock() {
			public void run() throws Throwable {
				Db4oClientServer.openServer(DATABASE_FILE, port);
			}
		});

	}
}