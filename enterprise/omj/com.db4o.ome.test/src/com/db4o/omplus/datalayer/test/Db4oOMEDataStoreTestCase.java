/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.datalayer.test;

import java.io.*;
import java.util.*;

import org.junit.*;
import static org.junit.Assert.*;

import com.db4o.omplus.datalayer.*;

public class Db4oOMEDataStoreTestCase {

	private static final String KEY = "key";
	private final static String DB_PATH = "omedata.test.db4o";

	private Db4oOMEDataStore dataStore;
	private MockPrefixProvider prefixProvider;
	
	@Before
	public void setUp() {
		deleteDBFile();
		prefixProvider = new MockPrefixProvider();
		dataStore = new Db4oOMEDataStore(DB_PATH, prefixProvider);
	}

	@After
	public void tearDown() {
		dataStore.close();
		deleteDBFile();
	}
	
	@Test
	public void testGlobal() {
		dataStore.setGlobalEntry(KEY, createEntry("value"));
		assertEntry("value", dataStore.<String>getGlobalEntry(KEY));
		dataStore.setGlobalEntry(KEY, createEntry("anothervalue"));
		assertEntry("anothervalue", dataStore.<String>getGlobalEntry(KEY));
	}

	@Test
	public void testContextLocal() {
		prefixProvider.prefix = "a";
		dataStore.setContextLocalEntry(KEY, createEntry("a"));
		prefixProvider.prefix = "b";
		dataStore.setContextLocalEntry(KEY, createEntry("b"));
		prefixProvider.prefix = "a";
		assertEntry("a", dataStore.<String>getContextLocalEntry(KEY));
		prefixProvider.prefix = "b";
		assertEntry("b", dataStore.<String>getContextLocalEntry(KEY));
	}

	private ArrayList<String> createEntry(String value) {
		ArrayList<String> entry = new ArrayList<String>();
		entry.add(value);
		return entry;
	}
	
	private void assertEntry(String expected, List<String> entry) {
		assertEquals(1, entry.size());
		assertEquals(expected, entry.get(0));
	}
	
	private void deleteDBFile() {
		new File(DB_PATH).delete();
	}
	
	private static class MockPrefixProvider implements ContextPrefixProvider {

		public String prefix = "";
		
		public String currentPrefix() {
			return prefix;
		}
		
	}
}
