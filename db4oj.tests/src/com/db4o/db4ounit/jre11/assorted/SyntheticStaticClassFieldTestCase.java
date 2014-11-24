/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.assorted;

import com.db4o.config.*;

import db4ounit.extensions.*;

public class SyntheticStaticClassFieldTestCase extends AbstractDb4oTestCase {

	protected void configure(Configuration config) throws Exception {
		config.objectClass(SyntheticStaticClassFieldData.class).persistStaticFieldValues();
	}
	
	protected void store() throws Exception {
		store(new SyntheticStaticClassFieldData());
	}

	public void test() {
		retrieveOnlyInstance(SyntheticStaticClassFieldData.class);
	}
}
