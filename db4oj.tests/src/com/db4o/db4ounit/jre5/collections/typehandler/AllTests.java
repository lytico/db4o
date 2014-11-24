/* Copyright (C) 2006 - 2007 Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections.typehandler;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oTestSuite {
    
    public static void main(String[] arguments) {
        new AllTests().runSolo();
    }
    
    protected Class[] testCases() {
        return 
            new Class[] {
		        ListTypeHandlerCascadedDeleteTestCase.class,
		        ListTypeHandlerPersistedCountTestCase.class,
				ListTypeHandlerTestSuite.class,
				ListTypeHandlerGreaterSmallerTestSuite.class,
				ListTypeHandlerStringElementTestSuite.class,
 				MapTypeHandlerTestSuite.class,
				NamedArrayListTypeHandlerTestCase.class,
				SimpleListTestCase.class,
				SimpleListQueryTestCase.class,
		};
	}

}
