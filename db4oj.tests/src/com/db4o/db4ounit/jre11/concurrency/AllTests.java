/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.concurrency;

import db4ounit.extensions.*;

public class AllTests extends Db4oConcurrencyTestSuite {
	
	public static void main(String[] args) {
		System.exit(new AllTests().runConcurrency());
	}

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.common.concurrency.AllTests.class,
			CascadeToExistingVectorMemberTestCase.class,
			Circular2TestCase.class,
			HashtableTestCase.class,
			HashtableModifiedUpdateDepthTestCase.class,
			NullWrapperQueriesTestCase.class,
		};
	}

}
