/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ext;

import com.db4o.ext.*;

import db4ounit.*;

public class Db4oDatabaseTestCase implements TestCase {
	
	public void testGenerate() {
		Db4oDatabase db1 = Db4oDatabase.generate();
		Db4oDatabase db2 = Db4oDatabase.generate();
		Db4oDatabase db3 = Db4oDatabase.generate();
		Assert.areNotEqual(db1, db2);
		Assert.areNotEqual(db1, db3);
		Assert.areNotEqual(db2, db3);
	}

}
