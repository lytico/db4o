package com.db4o.db4ounit.jre12.tp;

import db4ounit.extensions.Db4oTestSuite;

@decaf.Remove(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {
	
	@Override
	protected Class[] testCases() {
		return new Class[] {
			CollectionUpdateTPTestCase.class,
			ActivatableListUpdateTestCase.class,
		};
	}

}
