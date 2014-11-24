/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import java.lang.management.*;

import javax.management.*;

/**
 * JDK 1.5 compatible MBean "proxy".
 */
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class MBeanProxy {

	private final MBeanServer _platformServer = ManagementFactory.getPlatformMBeanServer();
	private final ObjectName _beanName;

	public MBeanProxy(ObjectName beanName) {
		_beanName = beanName;
	}

	public <T> T getAttribute(final String attribute) {
		try {
			return (T)_platformServer.getAttribute(_beanName, attribute);
		} catch (Exception e) {
			throw new IllegalStateException(e);
		}
	}
	
	public void resetCounters() {
		try {
			_platformServer.invoke(_beanName, "resetCounters", new Object[0], new String[0]);
		} catch (Exception e) {
			throw new IllegalStateException(e);
		}
	}

	public void addNotificationListener(NotificationListener listener, NotificationFilter notificationFilter) throws JMException {
		_platformServer.addNotificationListener(_beanName, listener, notificationFilter, null);
		
	}
}
