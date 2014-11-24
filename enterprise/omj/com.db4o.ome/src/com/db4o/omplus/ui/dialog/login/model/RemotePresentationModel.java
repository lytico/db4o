/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model;

import java.util.*;

import com.db4o.omplus.connection.*;

public class RemotePresentationModel extends ConnectionPresentationModel<RemoteConnectionParams> {

	private static final int MAX_PORT = 65535;

	private String host = "";
	private String port = "";
	private String user = "";
	private String pwd = "";
	
	public static interface RemoteSelectionListener {
		void remoteSelection(String host, String port, String user, String pwd);
	}

	private final List<RemoteSelectionListener> remoteListeners = new LinkedList<RemoteSelectionListener>();
	
	public RemotePresentationModel(LoginPresentationModel model, CustomConfigSource configSource) {
		super(model, configSource);
	}

	public void addRemoteSelectionListener(RemoteSelectionListener listener) {
		remoteListeners.add(listener);
	}

	public void host(String host) {
		if(this.host.equals(host)) {
			return;
		}
		this.host = host;
		newState();
		notifyListeners();
	}

	public void port(String port) {
		if(this.port.equals(port)) {
			return;
		}
		this.port = port;
		newState();
		notifyListeners();
	}

	public void user(String user) {
		if(this.user.equals(user)) {
			return;
		}
		this.user = user;
		notifyListeners();
	}

	public void password(String password) {
		if(this.pwd.equals(password)) {
			return;
		}
		this.pwd = password;
		notifyListeners();
	}

	@Override
	protected RemoteConnectionParams fromState(String[] jarPaths, String[] configNames) throws DBConnectException {
		host = trim(host, "host");
		port = trim(port, "port");
		int portNum = parsePortNumber(port);
		user = trim(user, "user");
		pwd = trim(pwd, "password");
		return new RemoteConnectionParams(host, portNum, user, pwd, jarPaths, configNames);
	}

	@Override
	protected void fromState(RemoteConnectionParams params) throws DBConnectException {
		user = trim(user, "user");
		pwd = trim(pwd, "password");
		params.user(user);
		params.password(pwd);
	}
	
	private String trim(String str, String name) throws DBConnectException {
		String trimmed = str == null ? "" : str.trim();
		if(trimmed.length() == 0) {
			throw new DBConnectException(name + " is empty");
		}
		return trimmed;
	}
	
	private static int parsePortNumber(String port) throws DBConnectException {
		String parsePort = port.toLowerCase();
		int radix = 10;
		if(parsePort.startsWith("0x")) {
			parsePort = parsePort.substring(2);
			radix = 16;
		}
		try {
			int portNum = Integer.parseInt(parsePort, radix);
			if(portNum < 0 || portNum > MAX_PORT) {
				throw new DBConnectException("Port not in range (0-" + MAX_PORT + "): " + portNum);
			}
			return portNum;
		}
		catch(NumberFormatException exc) {
			throw new DBConnectException("Illegal port: " + port, exc);
		}
	}

	@Override
	protected List<RemoteConnectionParams> connections(RecentConnectionList recentConnections) {
		return recentConnections.getRecentConnections(RemoteConnectionParams.class);
	}

	@Override
	protected void selected(RemoteConnectionParams remoteParams) {
		host = remoteParams.getHost();
		port = String.valueOf(remoteParams.getPort());
		user = remoteParams.getUser();
		pwd = remoteParams.getPassword();
		notifyListeners();
	}

	@Override
	protected void clearSpecificState() {
		host("");
		port("");
		user("");
		password("");
	}


	private void notifyListeners() {
		for (RemoteSelectionListener listener : remoteListeners) {
			listener.remoteSelection(host, port, user, pwd);
		}
	}

}
