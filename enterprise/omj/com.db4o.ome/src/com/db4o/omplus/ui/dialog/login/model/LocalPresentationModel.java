/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model;

import java.util.*;

import com.db4o.omplus.connection.*;

public class LocalPresentationModel extends ConnectionPresentationModel<FileConnectionParams> {

	private String path = "";
	private boolean readOnly = false;
	
	public static interface LocalSelectionListener {
		void localSelection(String path, boolean readOnly);
	}

	private final List<LocalSelectionListener> localListeners = new LinkedList<LocalSelectionListener>();

	public LocalPresentationModel(LoginPresentationModel model, CustomConfigSource configSource) {
		super(model, configSource);
	}

	public void addLocalSelectionListener(LocalSelectionListener listener) {
		localListeners.add(listener);
	}
	
	public void path(String path) {
		if(this.path.equals(path)) {
			return;
		}
		this.path = path;
		newState();
		notifyListeners();
	}

	public void readOnly(boolean readOnly) {
		if(this.readOnly == readOnly) {
			return;
		}
		this.readOnly = readOnly;
		notifyListeners();
	}

	@Override
	protected FileConnectionParams fromState(String[] jarPaths, String[] configNames) throws DBConnectException {
		if(path == null || path.length() == 0) {
			throw new DBConnectException("Path is empty.");
		}
		return new FileConnectionParams(path, readOnly, jarPaths, configNames);
	}
	
	@Override
	protected void fromState(FileConnectionParams params) {
		params.readOnly(readOnly);
	}

	@Override
	protected List<FileConnectionParams> connections(RecentConnectionList recentConnections) {
		return recentConnections.getRecentConnections(FileConnectionParams.class);
	}

	@Override
	protected void selected(FileConnectionParams selected) {
		path = selected.getPath();
		readOnly = selected.readOnly();
		notifyListeners();
	}

	@Override
	protected void clearSpecificState() {
		path("");
		readOnly(false);
	}
	
	private void notifyListeners() {
		for (LocalSelectionListener listener : localListeners) {
			listener.localSelection(path, readOnly);
		}
	}

}
