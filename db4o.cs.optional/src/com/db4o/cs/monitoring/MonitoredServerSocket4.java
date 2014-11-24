/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import static com.db4o.foundation.Environments.*;

import java.io.*;

import javax.management.*;

import com.db4o.*;
import com.db4o.cs.foundation.*;
import com.db4o.events.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
class MonitoredServerSocket4 extends ServerSocket4Decorator {
	public MonitoredServerSocket4(ServerSocket4 serverSocket) {
		super(serverSocket);
	}

	public Socket4 accept() throws IOException {
		return new MonitoredServerSideClientSocket4(_serverSocket.accept(), bean());
	}
	
	Networking bean() {
		// FIXME
		if (_bean == null) {
			_bean = Db4oClientServerMBeans.newServerNetworkingStatsMBean(my(ObjectContainer.class));		
			try {
				_bean.register();
			} catch (JMException exc) {
				exc.printStackTrace();
			}
			unregisterBeanOnServerClose();			
		}
		return _bean;
	}

	private void unregisterBeanOnServerClose() {
		EventRegistry events = EventRegistryFactory.forObjectContainer(my(ObjectContainer.class));
		events.closing().addListener(new EventListener4<ObjectContainerEventArgs>() { public void onEvent(Event4<ObjectContainerEventArgs> e, ObjectContainerEventArgs args) {
			_bean.unregister();
			_bean = null;
		}});
	}
	
	private Networking _bean;	
}