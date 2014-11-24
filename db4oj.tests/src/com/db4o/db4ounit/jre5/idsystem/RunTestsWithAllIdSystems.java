/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.idsystem;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class RunTestsWithAllIdSystems implements TestSuiteBuilder {

	public Class testSuite() {
		return com.db4o.db4ounit.jre5.AllTestsDb4oUnitJdk5.class;
	}

	public static void main(String[] args) {
		Db4oTestSuite suite = new Db4oTestSuite() {
			protected Class[] testCases() {
				return new Class[] {
						RunTestsWithAllIdSystems.class, 
				};
			}

			protected Db4oTestSuiteBuilder soloSuite() {
				return new Db4oTestSuiteBuilder(
						new IdSystemFixture(), testCases());
			}

		};

		System.exit(suite.runAll());
	}

	public Iterator4 iterator() {
		return new Db4oTestSuiteBuilder(new IdSystemFixture(),
				testSuite()).iterator();
	}

}
