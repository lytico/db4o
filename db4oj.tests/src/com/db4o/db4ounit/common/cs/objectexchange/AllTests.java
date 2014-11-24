package com.db4o.db4ounit.common.cs.objectexchange;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			SlotCollectorTestCase.class,
		};
	}

}
