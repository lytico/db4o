/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.tests.fixtures;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(AllTests.class).run();
	}

	protected Class[] testCases() {
		return new Class[] {
			FixtureBasedTestSuiteTestCase.class,
			FixtureContextTestCase.class,
			Set4TestSuite.class,
		};
	}

}
