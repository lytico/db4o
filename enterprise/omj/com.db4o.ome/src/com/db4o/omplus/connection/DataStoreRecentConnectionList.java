package com.db4o.omplus.connection;

import java.util.*;

import com.db4o.omplus.datalayer.*;

public class DataStoreRecentConnectionList implements RecentConnectionList {
	
	private final static String LOCAL = "LOCAL_CONN";
	private final static String REMOTE = "REMOTE_CONN";

	private final OMEDataStore dataStore;

	public DataStoreRecentConnectionList(OMEDataStore dataStore) {
		this.dataStore = dataStore;
	}

	public <T extends ConnectionParams> List<T> getRecentConnections(Class<T> paramType) {
		List<T> connections = connections(paramType);
		return Collections.unmodifiableList(connections);
	}

	public <T extends ConnectionParams> void addNewConnection(T params) {
		if (params == null) {
			return;
		}
		List<T> connections = connections(params.getClass());
		if(connections.contains(params)) {
			if(params.equals(connections.get(0))) {
				return;
			}
			connections.remove(params);
		}
		connections.add(0, params);
		connections((Class<T>)params.getClass(), connections);
	}

	private <T extends ConnectionParams> List<T> connections(Class<?> paramType) {
		String key = key(paramType);
		List<T> connections = dataStore.getGlobalEntry(key);
		if(connections == null) {
			connections = new LinkedList<T>();
			dataStore.setGlobalEntry(key, connections);
		}
		return connections;
	}

	private String key(Class<?> paramType) {
		if(paramType == FileConnectionParams.class) {
			return LOCAL;
		}
		if(paramType == RemoteConnectionParams.class) {
			return REMOTE;
		}
		return paramType.getName();
	}
	
	private <T extends ConnectionParams> void connections(Class<T> paramType, List<T> list){
		dataStore.setGlobalEntry(key(paramType), list);
	}
}
