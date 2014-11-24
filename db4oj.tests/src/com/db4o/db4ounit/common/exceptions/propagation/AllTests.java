/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runAll();
	}

	protected Class[] testCases() {
		return composeTests(
				new Class[] {
						ExceptionDuringTopLevelCallTestSuite.class, 
				});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	protected Class[] composeWith() {
		return new Class[] {
				com.db4o.db4ounit.common.exceptions.propagation.cs.AllTests.class,				
		};
	}

}
