/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.defragment;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return new Class[] {
				DefragmentPrimitiveArrayInCollectionTestCase.class,
				DefragmentSkipClassTestCase.class,
				DefragUnknownClassTestCase.class,
		};
	}
}
