/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.foundation.network;

import com.db4o.foundation.*;

import db4ounit.*;


public class AllTests implements TestSuiteBuilder {
	
	public Iterator4 iterator() {
		return new ReflectionTestSuiteBuilder(new Class[] {
			NetworkSocketTestCase.class,
		}).iterator();	
	}
	
	public static void main(String[] args) {
		new ConsoleTestRunner(AllTests.class).run();
	}

}
