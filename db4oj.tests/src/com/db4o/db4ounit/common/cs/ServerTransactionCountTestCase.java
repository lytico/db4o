/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;


import com.db4o.cs.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.*;
import com.db4o.io.*;

import db4ounit.*;

public class ServerTransactionCountTestCase implements TestCase {
	
	private static final int TIMEOUT = 100;

	public void test() throws Exception{
		ServerConfiguration config = Db4oClientServer.newServerConfiguration();
		config.timeoutServerSocket(TIMEOUT);
		config.file().storage(new MemoryStorage());
		ObjectServerImpl server = (ObjectServerImpl) Db4oClientServer.openServer(config, "", Db4oClientServer.ARBITRARY_PORT);
		Thread.sleep(TIMEOUT * 2);
		Assert.areEqual(0, server.transactionCount());
		server.close();
	}

}
