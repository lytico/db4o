/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.regression;

import db4ounit.extensions.*;


public class AllTests extends ComposibleTestSuite {

	protected Class[] testCases() {
		return composeTests(
				new Class[] {
						Case1207TestCase.class,
						COR57TestCase.class,
						SetRollbackTestCase.class,
				});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
					COR234TestCase.class,
				};
	}
	
	public static void main(String[] args) {
		new AllTests().runSolo();
	}

}
