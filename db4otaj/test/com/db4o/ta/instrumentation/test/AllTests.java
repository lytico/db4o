/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test;

import com.db4o.ta.instrumentation.test.collections.*;
import com.db4o.ta.instrumentation.test.integration.*;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {
	
	public static void main(String[] args) {
		System.exit(new AllTests().runSolo());
	}

	protected Class[] testCases() {
		return new Class[] {
			ArrayListInstantiationInstrumentationTestCase.class,
			Db4oJarEnhancerTestCase.class,
			EnumTestCase.class,
			HashMapInstantiationInstrumentationTestCase.class,
			HashtableInstantiationInstrumentationTestCase.class,
			StackInstantiationInstrumentationTestCase.class,
			TransparentPersistenceClassLoaderTestCase.class,
			TransparentActivationInstrumentationIntegrationTestCase.class,
			TACollectionFileEnhancerTestSuite.class,
			TAFileEnhancerTestCase.class,
		};
	}

}
