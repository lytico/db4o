/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.tests;

import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class UnhandledExceptionInThreadTestCase implements TestCase {
	
	public static class ExceptionThrowingTestCase extends AbstractDb4oTestCase {
		public void test() {
			container().threadPool().start(ReflectPlatform.simpleName(UnhandledExceptionInThreadTestCase.class)+" Throwing Exception Thread", new Runnable() {
				public void run() {
					throw new IllegalStateException();
				}
			});
		}
	}
	
	public void testSolo() {
		
		final Db4oTestSuiteBuilder suite = new Db4oTestSuiteBuilder(new Db4oInMemory(), ExceptionThrowingTestCase.class);
		final TestResult result = new TestResult();
		new TestRunner(suite).run(result);
		Assert.areEqual(1, result.failures().size());
		
	}
}
