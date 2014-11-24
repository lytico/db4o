/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.defragment;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
        
        return new Class[] {
            SlotDefragmentVectorTestCase.class,
            SlotDefragmentVectorUUIDTestCase.class,
		};
    }
}
