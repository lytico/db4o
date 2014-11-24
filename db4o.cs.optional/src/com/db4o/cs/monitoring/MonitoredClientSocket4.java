/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import static com.db4o.foundation.Environments.*;

import com.db4o.*;
import com.db4o.cs.foundation.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class MonitoredClientSocket4 extends MonitoredSocket4Base {

	protected MonitoredClientSocket4(Socket4 socket) {
		super(socket);		
	}
	
	@Override
	protected Networking bean() {
		if (null == _bean)
		{
			_bean = Db4oClientServerMBeans.newClientNetworkingStatsMBean(my(ObjectContainer.class)); 
		}
		
		return _bean; 
	}	
	
	private Networking _bean;
}
