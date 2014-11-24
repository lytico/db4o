package com.db4o.db4ounit.common.tp;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
		return new Class[] {
			CommitTimeStampsWithTPTestCase.class,
			DeactivateDeletedObjectOnRollbackStrategyTestCase.class,
			QueryConsistencyTestCase.class,
			RollbackStrategyTestCase.class,
			TransparentPersistenceTestCase.class,
		};
	}

}
