/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] args) {
		System.exit(new AllTests().runAll());
	}

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.common.AllTests.class,
			com.db4o.db4ounit.jre11.assorted.AllTests.class,
            com.db4o.db4ounit.jre11.btree.AllTests.class,
            com.db4o.db4ounit.jre11.defragment.AllTests.class,
            com.db4o.db4ounit.jre11.events.AllTests.class,
            com.db4o.db4ounit.jre11.handlers.AllTests.class,
            com.db4o.db4ounit.jre11.internal.AllTests.class,
            com.db4o.db4ounit.jre11.soda.AllTests.class,
            com.db4o.db4ounit.jre11.types.AllTests.class,
		};
	}
}
