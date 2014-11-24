/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.datalayer;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;

public class Db4oOMEDataStore implements OMEDataStore {
	private final static String SEPARATOR ="/";
	
	private final ContextPrefixProvider prefixProvider;
	private  OMEData omeData;
	private ObjectContainer db;
	
	public Db4oOMEDataStore(String dbPath, ContextPrefixProvider prefixProvider){
		this.prefixProvider = prefixProvider;
		db = Db4oEmbedded.openFile(configure(), dbPath);
		omeData = readOMEData(db);
		if(omeData == null) {
			omeData = new OMEData();
		}
	}
		
	@SuppressWarnings("unchecked")
	public <T> List<T> getGlobalEntry(String key){
		if(key == null) {
			return null;
		}
		return omeData.data.get(key);
	}
	
	public <T> void setGlobalEntry(String key, List<T> list){
		if(key == null || list == null) {
			return;
		}
		omeData.data.put(key, list);
		writeData();
	}
	
	public <T> List<T> getContextLocalEntry(String key){
		return getGlobalEntry(getContextPrefixedKey(key));
	}
	
	public <T> void setContextLocalEntry(String key, List<T> list){
		setGlobalEntry(getContextPrefixedKey(key), list);
	}

	private String getContextPrefixedKey(String key) {
		return prefixProvider.currentPrefix() + SEPARATOR + key;
	}
	
	private void writeData() {
		db.store(omeData);
		db.commit();
	}

	private OMEData readOMEData(ObjectContainer db){
		ObjectSet<OMEData> result = db.query(OMEData.class);
		return result.hasNext() ? result.next() : null;
	}
	
	private EmbeddedConfiguration configure() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.common().objectClass(OMEData.class).minimumActivationDepth(Integer.MAX_VALUE);
		config.common().objectClass(OMEData.class).updateDepth(Integer.MAX_VALUE);
		config.common().allowVersionUpdates(true);
		return config;
	}
	
	public void close() {
		db.close();
	}
}
