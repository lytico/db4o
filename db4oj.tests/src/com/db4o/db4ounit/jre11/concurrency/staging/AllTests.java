/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.concurrency.staging;

import db4ounit.extensions.*;

public class AllTests extends Db4oConcurrencyTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runConcurrency();
	}

	protected Class[] testCases() {
		return new Class[] {
			ComparatorSortTestCase.class,
			CustomActivationDepthTestCase.class,
			CascadeDeleteArrayTestCase.class,
			CascadeToHashtableTestCase.class,
		
		};
	}

}
