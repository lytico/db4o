/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return composeTests(new Class[] {
				com.db4o.db4ounit.common.types.arrays.AllTests.class,
				StoreTopLevelPrimitiveTestCase.class,
				StringBuilderHandlerTestCase.class,
				UnmodifiableListTestCase.class,
		});
    }
	
	/** @sharpen.if !SILVERLIGHT */
	@Override
	protected Class[] composeWith() {
		return new Class[] { 
				StoreExceptionTestCase.class // Storing exceptions is not supported on Silverlight. 
		};
	}
}
