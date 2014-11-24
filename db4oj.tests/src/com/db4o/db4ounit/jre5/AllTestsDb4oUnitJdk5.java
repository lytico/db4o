/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5;

import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsDb4oUnitJdk5 extends Db4oTestSuite {

	public static void main(String[] args) {
//		System.exit(new AllTestsDb4oUnitJdk5().runSolo());
//		System.exit(new AllTestsDb4oUnitJdk5().runSoloAndEmbeddedClientServer());
		System.exit(new AllTestsDb4oUnitJdk5().runAll());
//		System.exit(new AllTestsDb4oUnitJdk5().runNetworking());
	}

	@Override
	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.common.assorted.AllTestsJdk5.class,
			com.db4o.db4ounit.jre5.annotation.AllTests.class,
			com.db4o.db4ounit.jre5.collections.AllTests.class,
			com.db4o.db4ounit.jre5.enums.AllTests.class,
			com.db4o.db4ounit.jre5.generic.AllTests.class,
			com.db4o.db4ounit.jre5.query.AllTests.class,
			com.db4o.db4ounit.jre12.AllTestsJdk1_2.class,
		};
	}

}
