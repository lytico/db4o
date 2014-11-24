/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import db4ounit.extensions.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.optional.monitoring.cs.AllTests.class,
			Db4oMBeanRegistryTestCase.class,
			FreespaceMonitoringSupportTestCase.class,
			MonitoredStorageTestCase.class,
			NativeQueryMonitoringSupportTestCase.class,
			ObjectLifecycleMonitoringSupportTestCase.class,
			QueryMonitoringSupportTestCase.class,
			ReferenceSystemMonitoringSupportTestCase.class,
		};
	}

}
