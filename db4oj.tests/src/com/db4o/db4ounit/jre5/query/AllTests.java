package com.db4o.db4ounit.jre5.query;

import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oTestSuite {
	public static void main(String[] args) {
		new AllTests().runAll();
	}
	
	@Override
	protected Class[] testCases() {
		return new Class[] {
			COR756TestCase.class,
			UntypedFieldTestCase.class,
		};
	}


}
