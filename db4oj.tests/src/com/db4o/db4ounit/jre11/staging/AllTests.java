/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre11.staging;

import com.db4o.db4ounit.jre11.events.*;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.common.staging.AllTests.class,
			
			CommittedCallbacksUpdateTestCase.class, // COR-594
			SQLDateTestCase.class, // COR-1989
			
			/**
			 *  When you add a test here, make sure you create a Jira issue. 
			 */
			
		};
	}
}
