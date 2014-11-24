/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre11.assorted;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return new Class[] {
			CascadeToHashtableTestCase.class,
			ClientServerThrowsOnCommitTestCase.class,
			NanoTimeTestCase.class,
            NullWrapperTestCase.class,
            ObjectNotStorableExceptionTestCase.class,
            ObjectNotStorableTestCase.class,
            StoreNumberTestCase.class,
            SyntheticStaticClassFieldTestCase.class,
		};
	}
}
