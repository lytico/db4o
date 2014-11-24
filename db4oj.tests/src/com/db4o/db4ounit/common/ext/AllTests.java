/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ext;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runAll();
	}

	protected Class[] testCases() {
		return composeTests(new Class[] {
								Db4oDatabaseTestCase.class,
								RefreshTestCase.class,
								StoredClassTestCase.class,
								StoredClassInstanceCountTestCase.class,
							});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
						TransientFieldRefreshNoClassesOnServerTestCase.class,
						UnavailableClassesWithTranslatorTestCase.class,
						UnavailableClassesWithTypeHandlerTestCase.class,
					};
	}
}
