/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.reflect;

import db4ounit.extensions.*;

public class GenericReflectorStateTest extends AbstractDb4oTestCase {
	
	protected void store() throws Exception {
	}
	
	public void testKnownClasses() {
		db().reflector().knownClasses();
	}
}
