/* Copyright (C) 2004 - 20067 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.cs.config;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runAll();
    }
	
	protected Class[] testCases() {
		return new Class[] {
				ClientConfigurationItemIntegrationTestCase.class,
				ClientConfigurationItemUnitTestCase.class,
				ClientServerConfigurationTestCase.class,
				ServerConfigurationItemIntegrationTestCase.class,
				ServerConfigurationItemUnitTestCase.class,
		};
	}
	
}
