/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.model;

import com.db4o.omplus.*;
import com.db4o.omplus.connection.*;

public class LoginPresentationModel {

	private final RecentConnectionList recentConnections;
	private final ErrorMessageHandler err;
	private final Connector connector;
	
	public LoginPresentationModel(RecentConnectionList recentConnections, ErrorMessageHandler err, Connector connector) {
		this.recentConnections = recentConnections;
		this.err = err;
		this.connector = connector;
	}
	
	RecentConnectionList recentConnections() {
		return recentConnections;
	}

	ErrorMessageHandler err() {
		return err;
	}
	
	void connect(ConnectionParams params) throws DBConnectException {
		try {
			connector.connect(params);
			recentConnections.addNewConnection(params);
		}
		catch(Exception exc) {
			throw new DBConnectException(params, ("Could not connect to " + params.getPath() + ": " + exc.getMessage()), exc);
		}
	}


}
