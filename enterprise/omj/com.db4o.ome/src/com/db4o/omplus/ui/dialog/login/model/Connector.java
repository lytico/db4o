/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.model;

import com.db4o.omplus.connection.*;

// FIXME dependency - action knows model
public interface Connector {
	boolean connect(ConnectionParams params) throws DBConnectException;
}