/* Copyright (C) 2006 - 2007 Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.facades;

import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {
    
    public static void main(String[] arguments) {
        new AllTests().runAll();
    }
    
	protected Class[] testCases() {
		return new Class[] {
				
		};
	}

}
