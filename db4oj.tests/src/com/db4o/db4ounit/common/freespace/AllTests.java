/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.freespace;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
        return composeTests(
        			new Class[] {
        					FreespaceManagerDiscardLimitTestCase.class,
        					FreespaceManagerReopenTestCase.class,
        					FreespaceManagerTestCase.class,
        					FreespaceManagerTypeChangeTestCase.class,
        					FreespaceRemainderLimitTestCase.class,
        			});
    }

	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
						BlockConfigurationFileSizeTestCase.class,
						FileSizeTestCase.class,
				};
	}
}
