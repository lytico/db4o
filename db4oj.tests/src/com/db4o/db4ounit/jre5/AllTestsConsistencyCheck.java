/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.jre5;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsConsistencyCheck {

	public static void main(String[] args) {
		Db4oTestSuite suite=new Db4oTestSuite() {
			protected Class[] testCases() {
				return new Class[] {
					AllTestsDb4oUnitJdk5.class,
				};
			}

			protected Db4oTestSuiteBuilder soloSuite() {
		        return new Db4oTestSuiteBuilder(
	                new Db4oConsistencyCheckSolo(), testCases());			}
			
		};
		suite.runSolo();
	}

}
