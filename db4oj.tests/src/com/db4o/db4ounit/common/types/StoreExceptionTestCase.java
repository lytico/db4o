/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types;

import db4ounit.extensions.*;

public class StoreExceptionTestCase extends AbstractDb4oTestCase{
	
	// The following failed with JDK7 before the UnmodifiableListTypeHandler was introduced.
	public void test(){
		Exception e = new Exception();
		db().store(e);
	}

}
