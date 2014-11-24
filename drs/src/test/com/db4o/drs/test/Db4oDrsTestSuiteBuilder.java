/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
package com.db4o.drs.test;

import com.db4o.foundation.*;

import db4ounit.*;

/** @sharpen.partial **/
public class Db4oDrsTestSuiteBuilder implements TestSuiteBuilder {
	
	public static void main(String[] args) {
		int errorCount = runTests();	
		exit(errorCount);
	}

	/** @sharpen.ignore **/
	private static void exit(int errorCount) {
		System.exit(errorCount);
	}

	public static int runTests() {
		return new ConsoleTestRunner(new Db4oDrsTestSuiteBuilder()).run();
	}
	
	public Iterator4 iterator() {
		
		if(false)
			return new DrsTestSuiteBuilder(
				new Db4oDrsFixture("db4o-a"),
				new Db4oDrsFixture("db4o-b"),
				Db4oDrsTestSuite.class).iterator();
		
		if(false)
			return new DrsTestSuiteBuilder(
					new Db4oClientServerDrsFixture("db4o-cs-a", 0xdb40), 
					new Db4oClientServerDrsFixture("db4o-cs-b", 4455),
					Db4oDrsTestSuite.class).iterator();
		
		
		return Iterators.concat(
			Iterators.concat(
				new DrsTestSuiteBuilder(
					new Db4oDrsFixture("db4o-a"),
					new Db4oDrsFixture("db4o-b"),
					Db4oDrsTestSuite.class).iterator(),
				new DrsTestSuiteBuilder(
					new Db4oClientServerDrsFixture("db4o-cs-a", 0xdb40), 
					new Db4oClientServerDrsFixture("db4o-cs-b", 4455),
					Db4oDrsTestSuite.class).iterator()),
			Iterators.concat(
				new DrsTestSuiteBuilder(
					new Db4oDrsFixture("db4o-a"),
					new Db4oClientServerDrsFixture("db4o-cs-b", 4455),
					Db4oDrsTestSuite.class).iterator(),
			
				new DrsTestSuiteBuilder(
					new Db4oClientServerDrsFixture("db4o-cs-a", 4455), 
					new Db4oDrsFixture("db4o-b"), 
					Db4oDrsTestSuite.class).iterator()));
	}

}
