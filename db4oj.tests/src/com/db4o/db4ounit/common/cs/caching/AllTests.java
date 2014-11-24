package com.db4o.db4ounit.common.cs.caching;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			ClientSlotCacheTestCase.class,
		};
	}

}
