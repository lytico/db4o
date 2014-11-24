/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ids;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
	}

	protected Class[] testCases() {
		return new Class[] { 
			IdSystemTestSuite.class,
		};
	}
}
