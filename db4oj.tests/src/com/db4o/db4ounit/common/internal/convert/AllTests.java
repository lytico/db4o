package com.db4o.db4ounit.common.internal.convert;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			ConverterTestCase.class,
		};
	}

}
