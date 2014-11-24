package com.db4o.db4ounit.common.staging;

import db4ounit.*;
import db4ounit.extensions.*;

public class StoredClassUnknownClassQueryTestCase extends AbstractDb4oTestCase {

	public static class UnknownClass {
		public int _id;
	}

	public void test() {
		final int numStoredClasses = db().storedClasses().length;
		Assert.isNull(db().storedClass(UnknownClass.class));
		Assert.areEqual(0, db().query(UnknownClass.class).size());
		Assert.isNull(db().storedClass(UnknownClass.class));
		Assert.areEqual(numStoredClasses, db().storedClasses().length);
	}
}
