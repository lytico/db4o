/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

import db4ounit.extensions.*;

public class AllCommonTATests extends Db4oTestSuite {
	
	public static void main(String[] args) {
		new AllCommonTATests().runAll();
	}
	
	protected Class[] testCases() {
		return new Class[] {
				
			ActivatableTestCase.class,
			ActivationBasedOnConcreteTypeTestCase.class,
			TransparentActivationSupportTestCase.class,
			TAWithGCBeforeCommitTestCase.class,
			com.db4o.db4ounit.common.ta.events.AllTests.class,
			com.db4o.db4ounit.common.ta.mixed.AllTests.class,
			com.db4o.db4ounit.common.ta.nonta.AllTests.class,
			com.db4o.db4ounit.common.ta.sample.AllTests.class,
			com.db4o.db4ounit.common.ta.ta.AllTests.class,
		};
	}

}
