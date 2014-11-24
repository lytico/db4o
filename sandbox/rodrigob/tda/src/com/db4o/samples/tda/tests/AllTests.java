package com.db4o.samples.tda.tests;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] args) {
	    new AllTests().runSolo();
    }

	@Override
	protected Class[] testCases() {
		return new Class[] {
			DeactivationOnExpirationTestCase.class,
			TimerMockTestCase.class
		};
	}

}
