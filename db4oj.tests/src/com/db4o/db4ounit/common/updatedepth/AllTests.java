/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.updatedepth;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	@Override
	protected Class[] testCases() {
		return new Class[] {
			NegativeUpdateDepthTestCase.class,
			UpdateDepthTestCase.class,
			UpdateDepthWithCascadingDeleteTestCase.class,
		};
	}

}
