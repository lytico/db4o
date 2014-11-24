/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.debug;

import java.util.*;

import com.db4o.omplus.datalayer.*;

// TODO move me to test project again
public class InMemoryOMEDataStore implements OMEDataStore {

	private Map<String, List<?>> localEntries = new HashMap<String, List<?>>();
	private Map<String, List<?>> globalEntries = new HashMap<String, List<?>>();
	
	public void close() {
	}

	@SuppressWarnings("unchecked")
	public <T> List<T> getContextLocalEntry(String key) {
		return (List<T>) localEntries.get(key);
	}

	@SuppressWarnings("unchecked")
	public <T> List<T> getGlobalEntry(String key) {
		return (List<T>) globalEntries.get(key);
	}

	public <T> void setContextLocalEntry(String key, List<T> list) {
		localEntries.put(key, list);
	}

	public <T> void setGlobalEntry(String key, List<T> list) {
		globalEntries.put(key, list);
	}

}
