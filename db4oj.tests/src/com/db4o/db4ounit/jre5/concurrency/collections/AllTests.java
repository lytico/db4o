/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre5.concurrency.collections;

import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTests extends Db4oConcurrencyTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runEmbeddedConcurrency();
	}

	protected Class[] testCases() {
		return new Class[] { 
			ArrayList4TestCase.class,
			ArrayMap4TestCase.class,
		};
	}

}
