/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre12.staging;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.jre11.staging.AllTests.class,
			
			/**
			 *  When you add a test here, make sure you create a Jira issue. 
			 */
            HashMapTestCase.class,
            DuplicatePrimitiveArrayTestCase.class,
			SerializableConstructorTestCase.class,
		};
	}
}
