/* Copyright (C) 2006 - 2007 Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] arguments) {
        new AllTests().runAll();
    }
	
	protected Class[] testCases() {
		return new Class[] {
				com.db4o.db4ounit.jre12.collections.transparent.list.AllTests.class,
				com.db4o.db4ounit.jre12.collections.transparent.set.AllTests.class,
				com.db4o.db4ounit.jre12.collections.transparent.map.AllTests.class,
		};
	}
}
