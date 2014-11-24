package db4ounit;

import com.db4o.foundation.*;

/**
 * Support for hierarchically chained test suites.
 * 
 * In the topmost test package define an AllTests class which extends
 * ReflectionTestSuite and returns all subpackage.AllTests classes as
 * testCases. Example:
 * 
 * package org.acme.tests;
 * 
 * public class AllTests extends ReflectionTestSuite {
 * 	protected Class[] testCases() {
 * 		return new Class[] {
 *			org.acme.tests.subsystem1.AllTests.class,
 *			org.acme.tests.subsystem2.AllTests.class,
 *		};
 *	} 			
 * }
 */
public abstract class ReflectionTestSuite implements TestSuiteBuilder {

	public Iterator4 iterator() {
		return new ReflectionTestSuiteBuilder(testCases()).iterator();
	}

	protected abstract Class[] testCases();
	
	public int run() {
		return new ConsoleTestRunner(iterator()).run();
	}

}
