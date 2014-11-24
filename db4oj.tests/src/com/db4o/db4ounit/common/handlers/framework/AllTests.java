package com.db4o.db4ounit.common.handlers.framework;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			InstantiatingTypeHandlerSemanticsTestCase.class,
			TypeHandlerHierarchySemanticsTestCase.class
		};
	}

}
