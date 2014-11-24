/* Copyright (C) 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.extensions.*;


/**
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsJdk5 extends Db4oTestSuite {

	public static void main(String[] args) {
		System.exit(new AllTestsJdk5().runAll());
    }

	protected Class[] testCases() {
		return new Class[] {
			ComparatorSortTestCase.class,
			ConstructorNotRequiredTestCase.class,
		};
	}
}
