/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.tests;

import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ExcludingReflectorTestCase implements TestCase {
	public static class Excluded {
	}

	public void testExcludedClasses() {
		assertNotVisible(Excluded.class);
	}
	
	private void assertNotVisible(Class type) {
		Assert.isNull(reflectClassForName(type.getName()));
	}

	private ReflectClass reflectClassForName(final String className) {
		return new ExcludingReflector(Excluded.class).forName(className);
	}

	public void testVisibleClasses() {		
		assertVisible(getClass());
	}
	
	private void assertVisible(Class type) {
		Assert.isNotNull(reflectClassForName(type.getName()));
	}
}
