/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.extensions.tests;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {
	
	public static void main(String[] args) {
		new AllTests().run();
	}

	protected Class[] testCases() {
		return new Class[] {
			Db4oClientServerFixtureTestCase.class,
			Db4oEmbeddedSessionFixtureTestCase.class,
			DynamicFixtureTestCase.class,
			ExcludingReflectorTestCase.class,
			FixtureConfigurationTestCase.class,
			FixtureTestCase.class,
			UnhandledExceptionInThreadTestCase.class,
		};
	}

}
