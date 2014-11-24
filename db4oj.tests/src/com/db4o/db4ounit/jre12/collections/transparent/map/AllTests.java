/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.map;


import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] arguments) {
        new AllTests().runAll();
    }
	
	protected Class[] testCases() {
		return new Class[] {
				ActivatableHashMapTestCase.class,
				ActivatableHashtableTestCase.class,
				ActivatableMapAPITestSuite.class,
		};
	}
}
