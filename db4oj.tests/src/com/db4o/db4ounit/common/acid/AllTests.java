/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.acid;

import db4ounit.extensions.*;


public class AllTests extends ComposibleTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runSolo();
	}
	
	protected Class[] testCases() {
		return composeWith();
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
//				CrashSimulatingTestSuite.class,
				ReadCommittedIsolationTestCase.class,
			};
	}
}
