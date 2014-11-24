/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.cs;

import com.db4o.cs.internal.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ServerClosedTestCase extends Db4oClientServerTestCase implements OptOutAllButNetworkingCS{
    
	public static void main(String[] args) {
		new ServerClosedTestCase().runAll();
	}

	public void test() throws Exception {
		final ExtObjectContainer db = fixture().db();
		
		ObjectServerImpl serverImpl = (ObjectServerImpl) clientServerFixture()
				.server();
		try {
			Iterator4 iter = serverImpl.iterateDispatchers();
			iter.moveNext();
			ServerMessageDispatcherImpl serverDispatcher = (ServerMessageDispatcherImpl) iter
					.current();
			serverDispatcher.socket().close();
			Runtime4.sleep(1000);
			Assert.expect(DatabaseClosedException.class, new CodeBlock() {
				public void run() throws Throwable {
					db.queryByExample(null);
				}
			});
			Assert.isTrue(db.isClosed());
		} finally {
			serverImpl.close();
		}
	}

}
