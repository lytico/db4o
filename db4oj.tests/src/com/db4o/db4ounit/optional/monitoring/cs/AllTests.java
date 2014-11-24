/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.optional.monitoring.cs;

import db4ounit.extensions.Db4oTestSuite;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oTestSuite {	
	
	@Override
	protected Class[] testCases() {
		return new Class[] {
				ClientConnectionsTestCase.class,
				MonitoredClientSocket4TestCase.class,
				MonitoredServerSocket4TestCase.class,
		};
	}
}
