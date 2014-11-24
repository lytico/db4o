/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.drs.test;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class Db4oDb4oDrsTestSuite implements TestSuiteBuilder, Db4oTestCase{

	public Iterator4 iterator() {
		return new DrsTestSuiteBuilder(new Db4oDrsFixture("db4o-a"),
				new Db4oDrsFixture("db4o-b"), Db4oDrsTestSuite.class).iterator();
	}

}
