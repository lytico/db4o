/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.monitoring.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class Db4oClientServerMBeans {

	public static Networking newClientNetworkingStatsMBean(ObjectContainer container) {
		try {
			return new Networking(container, NetworkingMBean.class);
		} catch (JMException e) {
			throw new Db4oException(e);
		}
	}

	public static Networking newServerNetworkingStatsMBean(ObjectContainer container) {
		try {
			return new SynchronizedNetworking(container, NetworkingMBean.class);
		} catch (JMException e) {
			throw new Db4oException(e);
		}
	}

	public static ClientConnections newClientConnectionsMBean(ObjectServer server) {
		try {
			return new ClientConnections(Db4oMBeans.mBeanNameFor(ClientConnectionsMBean.class, Db4oMBeans.mBeanIDForContainer(server.ext().objectContainer())));
		} catch (JMException e) {
			throw new Db4oIllegalStateException(e);
		}
	}

}
