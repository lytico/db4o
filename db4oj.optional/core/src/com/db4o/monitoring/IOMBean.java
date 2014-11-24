/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;


/**
 * JMX MBean for IO statistics.
 * 
 * @exclude
 * 
 * @see MonitoredStorage
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface IOMBean {

	double getBytesReadPerSecond();
	double getBytesWrittenPerSecond();
	double getReadsPerSecond();
	double getWritesPerSecond();
	double getSyncsPerSecond();
	
}
