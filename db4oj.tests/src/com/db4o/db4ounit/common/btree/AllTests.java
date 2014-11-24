/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
        
        return new Class[] {
            BTreeAddRemoveTestCase.class,
            BTreeAsSetTestCase.class,
            BTreeFreeTestCase.class,
            BTreeIteratorTestCase.class,
            BTreeNodeTestCase.class,
            BTreePointerTestCase.class,
            BTreeRangeTestCase.class,
            BTreeRollbackTestCase.class,
            BTreeSearchTestCase.class,
            BTreeSimpleTestCase.class,
            BTreeStructureChangeListenerTestCase.class,
            SearcherLowestHighestTestCase.class,
            SearcherTestCase.class,
		};
    }
}
