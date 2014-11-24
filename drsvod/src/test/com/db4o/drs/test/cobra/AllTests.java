/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.cobra;


import com.db4o.drs.test.versant.*;
import com.db4o.drs.test.versant.timestamp.*;

import db4ounit.*;

public class AllTests extends ReflectionTestSuite {
	
	public static void main(String[] args) {
		new AllTests().run();
	}

	protected Class[] testCases() {
		return new Class[] {
			VodCobraTestCase.class,  
			CobraObjectLifecycleTestCase.class,
			CobraReplicationSupportTestCase.class,
			TimestampGeneratorTestCase.class,
		};
	}

}
