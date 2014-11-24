package com.db4o.db4ounit.optional.handlers;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] args) {
	    new AllTests().runAll();
    }

	@Override
	protected Class[] testCases() {
		return new Class[] {
			BigNumbersTestSuite.class,
			UuidTypeHandlerTestCase.class
		};
	}

}
