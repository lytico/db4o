/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.datalayer.test;

import static org.easymock.EasyMock.*;
import static org.junit.Assert.*;

import java.util.*;

import org.junit.*;

import com.db4o.*;
import com.db4o.omplus.*;
import com.db4o.omplus.datalayer.*;

public class Db4oOMEDataStoreIntegrationTestCase {

	private static final String KEY = "key";

	@Test
	public void testAppDataStore() {
		OMEDataStore dataStore = Activator.getDefault().getOMEDataStore();
		DatabaseModel dbModel = Activator.getDefault().dbModel();
		assertNull(dataStore.getContextLocalEntry(KEY));
		ObjectContainer dbA = db();
		dbModel.connect(dbA, "a");
		dataStore.setContextLocalEntry(KEY, new ArrayList<String>());
		assertNotNull(dataStore.getContextLocalEntry(KEY));
		ObjectContainer dbB = db();
		dbModel.connect(dbB, "b");
		verify(dbA);
		assertNull(dataStore.getContextLocalEntry(KEY));
		ObjectContainer dbAA = db();
		dbModel.connect(dbAA, "a");
		verify(dbB);
		assertNotNull(dataStore.getContextLocalEntry(KEY));
		dbModel.disconnect();
		verify(dbAA);
	}

	private ObjectContainer db() {
		ObjectContainer db = createMock(ObjectContainer.class);
		expect(db.close()).andReturn(true);
		replay(db);
		return db;
	}
}
