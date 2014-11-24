/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class InvalidPasswordTestCase
	extends Db4oClientServerTestCase
	implements OptOutAllButNetworkingCS {
	
	public void testInvalidPassword() {
		final int port = clientServerFixture().serverPort();
		Assert.expect(InvalidPasswordException.class, new CodeBlock() {
			public void run() throws Throwable {
				openClient("127.0.0.1", port, "strangeusername",
						"invalidPassword");
			}
		});
	}
	
	protected ObjectContainer openClient(String host, int port, String user,
			String password) {
		return com.db4o.cs.Db4oClientServer.openClient(host, port, user, password);
	}

	public void testEmptyUserPassword() {
		final int port = clientServerFixture().serverPort();
		Assert.expect(InvalidPasswordException.class, new CodeBlock() {
			public void run() throws Throwable {
				openClient("127.0.0.1", port, "", "");
			}
		});
	}
	
	public void testEmptyUserNullPassword() {
		final int port = clientServerFixture().serverPort();
		Assert.expect(InvalidPasswordException.class, new CodeBlock() {
			public void run() throws Throwable {
				openClient("127.0.0.1", port, "", null);
			}
		});
	}
	
	public void testNullPassword() {
		final int port = clientServerFixture().serverPort();
		Assert.expect(InvalidPasswordException.class, new CodeBlock() {
			public void run() throws Throwable {
				openClient("127.0.0.1", port, null, null);
			}
		});
	}
}
