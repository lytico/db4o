/* Copyright (C) 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre5.concurrency;

import db4ounit.extensions.*;


/**
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class AllTestsJdk5 extends Db4oConcurrencyTestSuite {

	public static void main(String[] args) {
		System.exit(new AllTestsJdk5().runConcurrency());
    }

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.db4ounit.jre12.concurrency.AllTestsJdk1_2.class,
		};
	}
}
