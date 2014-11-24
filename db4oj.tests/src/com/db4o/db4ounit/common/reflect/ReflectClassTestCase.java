/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.reflect;

import com.db4o.internal.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

import db4ounit.*;

public class ReflectClassTestCase implements TestCase {
	public static void main(String[] args) {
		new ConsoleTestRunner(ReflectClassTestCase.class).run();
	}
	
	public void testNameIsFullyQualified() {
		assertFullyQualifiedName(getClass());
		assertFullyQualifiedName(GenericArrayClass.class);
		assertFullyQualifiedName(int[].class);
	}

	private void assertFullyQualifiedName(Class clazz) {
		final ReflectClass reflectClass = Platform4.reflectorForType(clazz).forClass(clazz);
		Assert.areEqual(ReflectPlatform.fullyQualifiedName(clazz), reflectClass.getName());
	}
}
