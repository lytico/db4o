/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.util.test;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return new Class[] {
		    JavaServicesTestCase.class,
			PermutingTestConfigTestCase.class,
		};
	}
}
