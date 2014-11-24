package com.db4o.enhance.test;

import com.db4o.ta.*;

import db4ounit.*;

public class Db4oEnhancerAntTaskTestCase implements TestCase {

	private final static Class<ToBeInstrumented> INSTRUMENTED_CLAZZ = ToBeInstrumented.class;

	private final static Class<NotToBeInstrumented> NOT_INSTRUMENTED_CLAZZ = NotToBeInstrumented.class;

	public static void main(String[] args) {
		new ConsoleTestRunner(Db4oEnhancerAntTaskTestCase.class).run();
	}

	public void test() throws Exception {
		Assert.isTrue(Activatable.class.isAssignableFrom(INSTRUMENTED_CLAZZ));
		Assert.isFalse(Activatable.class
				.isAssignableFrom(NOT_INSTRUMENTED_CLAZZ));
	}
}
