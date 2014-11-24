/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface Db4oMBeanRegistry {
	void add(Db4oMBean bean);
	void register();
	void unregister();
}
