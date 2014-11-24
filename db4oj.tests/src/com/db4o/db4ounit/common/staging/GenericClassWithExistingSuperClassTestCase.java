/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.staging;

import com.db4o.db4ounit.common.assorted.*;

public class GenericClassWithExistingSuperClassTestCase extends UnavailableClassTestCaseBase {

	public static class Super {
		public int _id;
	}
	
	public static class Sub extends Super {
	}

	@Override
	protected void store() throws Exception {
		store(new Sub());
	}
	
	public void testFieldAccess() throws Exception {
		reopenHidingClasses(Sub.class);
		retrieveOnlyInstance(Sub.class);
	}
	
}
