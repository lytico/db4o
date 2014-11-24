/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.tests.fixtures;

import db4ounit.*;
import db4ounit.fixtures.*;

public class Set4TestSuite extends FixtureBasedTestSuite {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(Set4TestSuite.class).run();
	}
	
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
			new SubjectFixtureProvider(
					new Deferred4() {
						public Object value() {
							return new CollectionSet4();
						}
					},
					new Deferred4() {
						public Object value() {
							return new HashtableSet4();
						}
				}),
			new MultiValueFixtureProvider(
					new Object[] {},
					new Object[] { "foo", "bar", "baz" },
					new Object[] { "foo" },
					new Object[] { new Integer(42), new Integer(-1) }
				),
		};
	}
	
	public Class[] testUnits() { 
		return new Class[] {
			Set4TestUnit.class,
//			Iterable4TestUnit.class,
		};
	}

}
