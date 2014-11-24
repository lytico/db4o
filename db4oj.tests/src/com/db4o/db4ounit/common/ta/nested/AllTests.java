/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.ta.nested;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {
	
	protected Class[] testCases() {
		return new Class[] {
				NestedClassesTestCase.class,
		};
	}
	
	public static void main(String[] args) {
		new AllTests().runAll();
	}

}
