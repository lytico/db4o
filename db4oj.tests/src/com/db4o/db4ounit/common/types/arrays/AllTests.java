/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return new Class[] {
				ArrayNOrderTestCase.class,
				ByteArrayTestCase.class,
				NestedArraysTestCase.class,
				SimpleStringArrayTestCase.class,
				SimpleTypeArrayInUntypedVariableTestCase.class,
				TypedArrayInObjectTestCase.class,
				TypedDerivedArrayTestCase.class,
		};
    }
}
