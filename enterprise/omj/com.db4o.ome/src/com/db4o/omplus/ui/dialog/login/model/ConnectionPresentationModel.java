/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model;

import java.util.*;

import com.db4o.omplus.connection.*;

public abstract class ConnectionPresentationModel<P extends ConnectionParams> implements CustomConfigSink {
	
	private final LoginPresentationModel model;
	private final CustomConfigSource configSource;
	
	private LoginPresentationState state;
	private final Set<ConnectionPresentationListener> listeners = new HashSet<ConnectionPresentationListener>();

	public ConnectionPresentationModel(LoginPresentationModel model, CustomConfigSource configSource) {
		this.model = model;
		this.configSource = configSource;
		this.state = new NewState();
	}

	public void addConnectionPresentationListener(ConnectionPresentationListener listener) {
		listeners.add(listener);
		state.notifyListener(listener);
	}
	
	public String[] recentConnections() {
		List<P> connections = connections();
		String[] paths = new String[connections.size()];
		for (int idx = 0; idx < paths.length; idx++) {
			paths[idx] = connections.get(idx).getPath();
		}
		return paths;
	}

	public boolean connect() {
		try {
			model.connect(state.params());
			return true;
		} 
		catch (DBConnectException exc) {
			model.err().error(exc);
			return false;
		}
	}
	
	public void select(int idx) {
		P selected = connections().get(idx);
		state(new SelectedState(selected));
		selected(selected);
	}
	
	public void requestCustomConfig() {
		state.requestCustomConfig();
	}
	
	public void customConfig(String[] jarFiles, String[] customConfigClassNames) {
		state.customConfig(jarFiles, customConfigClassNames);
		notifyListeners();
	}

	public void clear() {
		state(new NewState());
		clearSpecificState();
	}
	
	protected List<P> connections() {
		return connections(model.recentConnections());
	}
	
	protected void newState() {
		String[] jarPaths = state.jarPaths();
		String[] configNames = state.configNames();
		state(new NewState(Arrays.copyOf(jarPaths, jarPaths.length), Arrays.copyOf(configNames, configNames.length)));
	}

	private void newStateFrom(P template) {
		newState();
		selected(template);
	}
	
	protected abstract P fromState(String[] jarPaths, String[] configNames) throws DBConnectException;
	protected abstract void fromState(P params) throws DBConnectException;
	protected abstract void selected(P selected);
	protected abstract List<P> connections(RecentConnectionList recentConnections);
	protected abstract void clearSpecificState();
	
	private void notifyListeners() {
		for (ConnectionPresentationListener listener : listeners) {
			state.notifyListener(listener);
		}
	}

	private void state(LoginPresentationState state) {
		this.state = state;
		notifyListeners();
	}
	
	private abstract class LoginPresentationState {
		abstract P params() throws DBConnectException;
		abstract void requestCustomConfig();
		abstract void customConfig(String[] jarPaths, String[] customConfigClassNames);
		abstract void notifyListener(ConnectionPresentationListener listener);
		abstract String[] jarPaths();
		abstract String[] configNames();
	}

	private class SelectedState extends LoginPresentationState {
		private final P params;
		
		public SelectedState(P params) {
			this.params = params;
		}

		@Override
		P params() throws DBConnectException {
			fromState(params);
			return params;
		}
		
		@Override
		void customConfig(String[] jarPaths, String[] customConfigClassNames) {
			params.jarPaths(jarPaths);
			params.configuratorClassNames(customConfigClassNames);
		}

		@Override
		void requestCustomConfig() {
			configSource.requestCustomConfig(ConnectionPresentationModel.this, params.jarPaths(), params.configuratorClassNames());
		}

		@Override
		void notifyListener(ConnectionPresentationListener listener) {
			listener.connectionPresentationState(false, params.jarPathCount(), params.configuratorClassNameCount());
		}

		@Override
		String[] jarPaths() {
			return params.jarPaths();
		}

		@Override
		String[] configNames() {
			return params.configuratorClassNames();
		}
	}

	private class NewState extends LoginPresentationState {
		private String[] jarPaths = {};
		private String[] configNames = {};

		public NewState() {
			this(new String[0], new String[0]);
		}
		
		public NewState(String[] jarPaths, String[] configNames) {
			this.jarPaths = jarPaths;
			this.configNames = configNames;
		}
		
		@Override
		public P params() throws DBConnectException {
			return fromState(jarPaths, configNames);
		}

		@Override
		public void customConfig(String[] jarPaths, String[] customConfigClassNames) {
			this.jarPaths = jarPaths;
			this.configNames = customConfigClassNames;
		}

		@Override
		void requestCustomConfig() {
			configSource.requestCustomConfig(ConnectionPresentationModel.this, jarPaths, configNames);
		}

		@Override
		void notifyListener(ConnectionPresentationListener listener) {
			listener.connectionPresentationState(true, jarPaths.length, configNames.length);
		}

		@Override
		String[] jarPaths() {
			return jarPaths;
		}

		@Override
		String[] configNames() {
			return configNames;
		}
	}
	
	public static interface ConnectionPresentationListener {
		void connectionPresentationState(boolean editEnabled, int jarPathCount, int configuratorCount);
	}

}
