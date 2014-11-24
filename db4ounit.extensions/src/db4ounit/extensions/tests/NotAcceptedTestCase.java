/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.tests;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class NotAcceptedTestCase extends AbstractDb4oTestCase implements OptOutFromTestFixture {
	public void test() {
		Assert.fail("Opted out test should not be run.");
	}
}