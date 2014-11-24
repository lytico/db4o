/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.connection.test;

import static junit.framework.Assert.*;

import java.util.*;

import org.junit.*;

import com.db4o.omplus.connection.*;
import com.db4o.omplus.debug.*;

public class DataStoreRecentConnectionListTestCase {

	private RecentConnectionList recentConnections;
	
	@Before
	public void setUp() {
		recentConnections = new DataStoreRecentConnectionList(new InMemoryOMEDataStore());
	}
	
	@Test
	public void testRetrievalOrder() {
		recentConnections.addNewConnection(new FileConnectionParams("foo"));
		recentConnections.addNewConnection(new RemoteConnectionParams("host1", 0xdb40, "db4o", "db4o"));
		recentConnections.addNewConnection(new FileConnectionParams("bar"));
		recentConnections.addNewConnection(new RemoteConnectionParams("host2", 0xdb40, "db4o", "db4o"));
		List<FileConnectionParams> fileConnections = recentConnections.getRecentConnections(FileConnectionParams.class);
		assertEquals(2, fileConnections.size());
		assertEquals("bar", fileConnections.get(0).getPath());
		assertEquals("foo", fileConnections.get(1).getPath());
		List<RemoteConnectionParams> remoteConnections = recentConnections.getRecentConnections(RemoteConnectionParams.class);
		assertEquals(2, remoteConnections.size());
		assertEquals("host2", remoteConnections.get(0).getHost());
		assertEquals("host1", remoteConnections.get(1).getHost());
	}

	@Test
	public void testDuplicateEntry() {
		recentConnections.addNewConnection(new FileConnectionParams("foo"));
		recentConnections.addNewConnection(new RemoteConnectionParams("host1", 0xdb40, "db4o", "db4o"));
		recentConnections.addNewConnection(new FileConnectionParams("bar"));
		recentConnections.addNewConnection(new RemoteConnectionParams("host2", 0xdb40, "db4o", "db4o"));
		recentConnections.addNewConnection(new FileConnectionParams("foo"));
		recentConnections.addNewConnection(new RemoteConnectionParams("host1", 0xdb40, "db4o", "db4o"));
		List<FileConnectionParams> fileConnections = recentConnections.getRecentConnections(FileConnectionParams.class);
		assertEquals(2, fileConnections.size());
		assertEquals("foo", fileConnections.get(0).getPath());
		assertEquals("bar", fileConnections.get(1).getPath());
		List<RemoteConnectionParams> remoteConnections = recentConnections.getRecentConnections(RemoteConnectionParams.class);
		assertEquals(2, remoteConnections.size());
		assertEquals("host1", remoteConnections.get(0).getHost());
		assertEquals("host2", remoteConnections.get(1).getHost());
	}

}
