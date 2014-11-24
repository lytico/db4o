/* Copyright (C) 2006 - 2007 Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections;

import db4ounit.extensions.*;

@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oTestSuite {
    
    public static void main(String[] arguments) {
        new AllTests().runSolo();
    }
    
    protected Class[] testCases() {
		return new Class[] {
			ArrayList4SODATestCase.class,
			ArrayList4TAMultiClientsTestCase.class,
			ArrayList4TATestCase.class,
			ArrayList4TestCase.class,
			ArrayList4TransparentUpdateTestCase.class,
			ArrayMap4TAMultiClientsTestCase.class,
			ArrayMap4TATestCase.class,
			ArrayMap4TestCase.class,
			ArrayMap4TransparentUpdateTestCase.class,
	        SubArrayList4TestCase.class,
	        com.db4o.db4ounit.jre5.collections.typehandler.AllTests.class,
	        com.db4o.db4ounit.jre5.collections.fast.AllTests.class,
		};
	}

}
