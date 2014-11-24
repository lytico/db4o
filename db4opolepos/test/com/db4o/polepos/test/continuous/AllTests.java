/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.test.continuous;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			SpeedTicketPerformanceStrategyTestCase.class,
			PerformanceMonitoringReporterTestCase.class,	
			RevisionBasedMostRecentJarFileSelectionStrategyTestCase.class,
		};
	}



}
