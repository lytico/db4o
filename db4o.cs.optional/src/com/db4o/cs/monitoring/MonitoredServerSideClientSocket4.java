/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import com.db4o.cs.foundation.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class MonitoredServerSideClientSocket4 extends MonitoredSocket4Base {

	public MonitoredServerSideClientSocket4(Socket4 socket, Networking bean) {
		super(socket);
		
		_bean = bean;
	}

	@Override
	protected Networking bean() {
		return _bean;
	}

	private Networking _bean;
}
