/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.api;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runSolo();
	}

	@Override
	protected Class[] testCases() {
		return composeWith();
	}

	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	protected Class[] composeWith() {
		return new Class[] {
				ClientConfigurationTestCase.class,
				CommonAndLocalConfigurationTestSuite.class,
				Db4oClientServerTestCase.class,
				Db4oEmbeddedTestCase.class,
				EnvironmentConfigurationTestCase.class,
				StoreAllTestCase.class,
		};
	}	
}
