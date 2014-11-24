package com.db4o.db4ounit.common.ta;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.common.ta.diagnostics.AllTests.class,
			com.db4o.db4ounit.common.ta.hierarchy.AllTests.class,
			com.db4o.db4ounit.common.ta.nested.AllTests.class,
			ReentrantActivationTestCase.class,
			TPExplicitStoreFieldIndexConsistencyTestCase.class,
		};
	}

}
