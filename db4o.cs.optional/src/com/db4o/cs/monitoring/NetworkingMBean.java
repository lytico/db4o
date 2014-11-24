/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.cs.monitoring;

import com.db4o.monitoring.*;

import decaf.*;

/**
 * @exclude
 * JMX MBean for networking IO statistics.
 * 
 * @see MonitoredSocket4Factory
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public interface NetworkingMBean extends ResettableMBean {
	
	double getBytesSentPerSecond();
	
	double getBytesReceivedPerSecond();
	
	double getMessagesSentPerSecond();

}
