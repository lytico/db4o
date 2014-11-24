/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.defragment;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public abstract class AbstractDb4oDefragTestCase implements TestSuiteBuilder {

	public String getLabel() {
		return "DefragAllTestCase: " +  testSuite().getName();
	}
	
	public abstract Class testSuite();

	public Iterator4 iterator() {
		return new Db4oTestSuiteBuilder(
				new Db4oDefragSolo(), testSuite()).iterator();
	}
}
