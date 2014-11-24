/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5.enums;


import db4ounit.extensions.*;



/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runSoloAndClientServer();
	}
	
	@Override
	protected Class[] testCases() {
		return new Class[] {
			DeleteEnumTestCase.class,
			EnumTestSuite.class,
			EnumUpdateTestCase.class,
			OrderByWithEnumsTestCase.class,
			SimpleEnumTestCase.class,
			TAEnumsTestCase.class,
		};
	}

}
