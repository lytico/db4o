/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import javax.management.*;

import com.db4o.monitoring.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class ClientConnections extends MBeanRegistrationSupport implements ClientConnectionsMBean {

	public ClientConnections(ObjectName name) throws JMException {
		super(name);
	}

	public int getConnectedClientCount() {
		synchronized (_connectedClientsLock) {
			return _connectedClients;
		}
	}
	
	public void notifyClientConnected() {
		synchronized(_connectedClientsLock) {
			_connectedClients++;
		}
	}
	
	public void notifyClientDisconnected() {
		synchronized(_connectedClientsLock) {
			_connectedClients--;
		}
	}
	
	private int _connectedClients;	
	private final Object _connectedClientsLock = new Object();
	
}
