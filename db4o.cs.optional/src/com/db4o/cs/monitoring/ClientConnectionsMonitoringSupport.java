/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.cs.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.*;
import com.db4o.events.*;
import com.db4o.ext.*;

import decaf.*;

/**
 * publishes the number of client connections to JMX.
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class ClientConnectionsMonitoringSupport implements ServerConfigurationItem {

	public void apply(ObjectServer server) {
		try {
			final ClientConnections bean = Db4oClientServerMBeans.newClientConnectionsMBean(server);
			bean.register();
			((ObjectServerEvents)server).closed().addListener(new EventListener4<ServerClosedEventArgs>() {
				public void onEvent(Event4<ServerClosedEventArgs> e, ServerClosedEventArgs args) {
					bean.unregister();
				}
			});
			
			((ObjectServerEvents)server).clientConnected().addListener(new EventListener4<ClientConnectionEventArgs>() { public void onEvent(Event4<ClientConnectionEventArgs> e, ClientConnectionEventArgs args) {
				bean.notifyClientConnected();
			}});
			
			((ObjectServerEvents)server).clientDisconnected().addListener(new EventListener4<StringEventArgs>() { public void onEvent(Event4<StringEventArgs> e, StringEventArgs args) {
				bean.notifyClientDisconnected();
			}});
		} 
		catch (JMException exc) {
			throw new Db4oException("Error setting up client connection monitoring support for " + server + ".", exc);
		}
	}

	public void prepare(ServerConfiguration configuration) {
	}
	
}
