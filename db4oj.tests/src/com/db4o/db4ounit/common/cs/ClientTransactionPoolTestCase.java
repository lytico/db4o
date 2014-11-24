/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.internal.*;
import com.db4o.io.*;

import db4ounit.*;

public class ClientTransactionPoolTestCase implements TestLifeCycle {

	public void testPool() {
		Configuration config = Db4o.newConfiguration();
		config.storage(new MemoryStorage());
		final LocalObjectContainer db = (LocalObjectContainer) Db4o.openFile(config, ClientTransactionTestUtil.MAINFILE_NAME);
		final ClientTransactionPool pool = new ClientTransactionPool(db);
		try {
			Assert.areEqual(0, pool.openTransactionCount());
			Assert.areEqual(1, pool.openFileCount());
			Transaction trans1 = pool.acquire(ClientTransactionTestUtil.MAINFILE_NAME);
			Assert.areEqual(db, trans1.container());			
			Assert.areEqual(1, pool.openTransactionCount());
			Assert.areEqual(1, pool.openFileCount());
			Transaction trans2 = pool.acquire(ClientTransactionTestUtil.FILENAME_A);
			Assert.areNotEqual(db, trans2.container());			
			Assert.areEqual(2, pool.openTransactionCount());
			Assert.areEqual(2, pool.openFileCount());
			Transaction trans3 = pool.acquire(ClientTransactionTestUtil.FILENAME_A);
			Assert.areEqual(trans2.container(), trans3.container());
			Assert.areEqual(3, pool.openTransactionCount());
			Assert.areEqual(2, pool.openFileCount());
			pool.release(ShutdownMode.NORMAL, trans3, true);
			Assert.areEqual(2, pool.openTransactionCount());
			Assert.areEqual(2, pool.openFileCount());
			pool.release(ShutdownMode.NORMAL, trans2, true);
			Assert.areEqual(1, pool.openTransactionCount());
			Assert.areEqual(1, pool.openFileCount());
			
			
		}
		finally {
			Assert.isFalse(db.isClosed());
			Assert.isFalse(pool.isClosed());
			pool.close();
			Assert.isTrue(db.isClosed());
			Assert.isTrue(pool.isClosed());
			Assert.areEqual(0, pool.openFileCount());
		}
	}

	public void setUp() throws Exception {
		ClientTransactionTestUtil.deleteFiles();
	}

	public void tearDown() throws Exception {
		ClientTransactionTestUtil.deleteFiles();
	}
}
