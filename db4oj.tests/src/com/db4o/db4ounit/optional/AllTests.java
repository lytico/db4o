/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	@Override
	protected Class[] testCases() {
		return composeTests(
				new Class[] {
						com.db4o.db4ounit.optional.handlers.AllTests.class,
						com.db4o.db4ounit.optional.monitoring.AllTests.class,
						BigMathSupportTestCase.class,
						ConsistencyCheckerTestSuite.class,
						UuidSupportTestCase.class, 
				} );
	}
	
	/** @sharpen.if !SILVERLIGHT */
	@Override
	protected Class[] composeWith() {
		return new Class[] { FileUsageStatsTestCase.class, };
	}

}
