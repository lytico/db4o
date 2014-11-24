/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11;

import db4ounit.extensions.*;

public class AllTestsOptional extends Db4oTestSuite {
	
	public static void main(String[] args) {
		System.exit(new AllTestsOptional().runAll());
	}

	protected Class[] testCases() {
		return new Class[] {
				com.db4o.db4ounit.jre11.tools.AllTests.class,
		};
	}
}
