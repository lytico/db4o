package com.db4o.instrumentation.test.bloat;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {

	public static void main(String[] args) {
		new ConsoleTestRunner(AllTests.class).run();
	}

	protected Class[] testCases() {
		return new Class[] {
			BloatReferenceProviderTestCase.class,
		};
	}
}
