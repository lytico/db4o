/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.internal.*;
import com.db4o.io.*;

import db4ounit.*;

public class ClientTransactionHandleTestCase implements TestLifeCycle {
		
	public void testHandles() {
		Configuration config = Db4o.newConfiguration();
		config.storage(new MemoryStorage());
		final LocalObjectContainer db = (LocalObjectContainer) Db4o.openFile(config, ClientTransactionTestUtil.MAINFILE_NAME);
		final ClientTransactionPool pool = new ClientTransactionPool(db);
		try {
			ClientTransactionHandle handleA = new ClientTransactionHandle(pool);
			Assert.areEqual(db, handleA.transaction().container());
			ClientTransactionHandle handleB = new ClientTransactionHandle(pool);
			Assert.areNotEqual(handleA.transaction(), handleB.transaction());
			Assert.areEqual(db, handleB.transaction().container());
			Assert.areEqual(2, pool.openTransactionCount());
			Assert.areEqual(1, pool.openFileCount());
			
			handleA.acquireTransactionForFile(ClientTransactionTestUtil.FILENAME_A);
			Assert.areEqual(3, pool.openTransactionCount());
			Assert.areEqual(2, pool.openFileCount());
			Assert.areNotEqual(db, handleA.transaction().container());
			handleB.acquireTransactionForFile(ClientTransactionTestUtil.FILENAME_A);
			Assert.areEqual(4, pool.openTransactionCount());
			Assert.areEqual(2, pool.openFileCount());
			Assert.areNotEqual(handleA.transaction(), handleB.transaction());
			Assert.areEqual(handleA.transaction().container(), handleB.transaction().container());
			
			handleA.releaseTransaction(ShutdownMode.NORMAL);
			Assert.areEqual(db, handleA.transaction().container());
			Assert.areNotEqual(db, handleB.transaction().container());
			Assert.areEqual(3, pool.openTransactionCount());
			Assert.areEqual(2, pool.openFileCount());
			handleB.releaseTransaction(ShutdownMode.NORMAL);
			Assert.areEqual(db, handleB.transaction().container());
			Assert.areEqual(2, pool.openTransactionCount());
			Assert.areEqual(1, pool.openFileCount());
			
			handleB.close(ShutdownMode.NORMAL);
			Assert.areEqual(1, pool.openTransactionCount());
			
			handleA.close(ShutdownMode.NORMAL);
			Assert.areEqual(0, pool.openTransactionCount());
			
		}
		finally {
			pool.close();
		}
	}

	public void setUp() throws Exception {
		ClientTransactionTestUtil.deleteFiles();
	}

	public void tearDown() throws Exception {
		ClientTransactionTestUtil.deleteFiles();
	}

}
