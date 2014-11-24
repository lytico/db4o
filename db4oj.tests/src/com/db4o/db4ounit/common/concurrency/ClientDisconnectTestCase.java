/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.cs.internal.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ClientDisconnectTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] arguments) {
        new ClientDisconnectTestCase().runConcurrency();
        new ClientDisconnectTestCase().runConcurrency();
	}
	
	public void _concDelete(ExtObjectContainer oc, int seq) throws Exception {
		final ClientObjectContainer client = (ClientObjectContainer) oc;
		try {
			if (seq % 2 == 0) {
				// ok to get something
				client.queryByExample(null);
			} else {
				client.socket().close();
				Assert.isFalse(oc.isClosed());
				Assert.expect(Db4oException.class, new CodeBlock() {
					public void run() throws Throwable {
						client.queryByExample(null);	
					}
				});
			}
		} finally {
			oc.close();
			Assert.isTrue(oc.isClosed());
		}
	}
}
