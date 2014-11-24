/* Copyright (C) 2004 - 2007 Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.cs.*;
import com.db4o.cs.internal.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;

import db4ounit.*;

public class CloseServerBeforeClientTestCase extends TestWithTempFile {

	public static void main(String[] arguments) {
		for (int i = 0; i < 1000; i++)
			new ConsoleTestRunner(CloseServerBeforeClientTestCase.class).run();
	}

	public void test() throws Exception {
		ObjectServer server = Db4oClientServer.openServer(tempFile(), Db4oClientServer.ARBITRARY_PORT);
		server.grantAccess("", "");
		
		ObjectContainer client = Db4oClientServer.openClient("localhost", ((ObjectServerImpl)server).port(), "", "");
		ObjectContainer client2 = Db4oClientServer.openClient("localhost", ((ObjectServerImpl)server).port(), "", "");
		
		client.commit();
		client2.commit();
		
		try {
			server.close();
		} finally {
			try{
				client.close();
				client2.close();
			} catch(Db4oException e) {
				// database may have been closed
			}
		}
	}
}
