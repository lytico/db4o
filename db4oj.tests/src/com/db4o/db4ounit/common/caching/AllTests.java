package com.db4o.db4ounit.common.caching;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			CacheTestSuite.class,
			PurgeableCacheTestCase.class,
		};
	}

}
