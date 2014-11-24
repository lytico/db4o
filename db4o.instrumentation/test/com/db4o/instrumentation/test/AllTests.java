package com.db4o.instrumentation.test;

import com.db4o.instrumentation.test.classfilter.*;
import com.db4o.instrumentation.test.core.*;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {

	public static void main(String[] args) {
		new ConsoleTestRunner(AllTests.class).run();
	}

	protected Class[] testCases() {
		return new Class[] {
			com.db4o.instrumentation.test.bloat.AllTests.class,
			DefaultFilePathRootTestCase.class,
			JarFileClassFilterTestCase.class,
		};
	}
}
