/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import db4ounit.extensions.*;

/**
 */
@decaf.Remove(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {

    public static void main(String[] arguments) {
		new AllTests().runAll();
    }

    protected Class[] testCases() {
		return new Class[] {
			ArrayListElementQueriesTestCase.class,
			ArrayListInHashMapTestCase.class,
			ArrayListCandidatesTestCase.class,
			BigSetTestCase.class,
			ByteArrayAsMapValueTestCase.class,
			CascadeDeleteCollectionTestCase.class,
			CascadeDeleteSubClassTestCase.class,
			CascadeToHashMapTestCase.class,
        	CollectionUuidTest.class,
        	CollectionContainsTestCase.class,
        	DeleteFromMapTestCase.class,
        	HashMapActivationTestCase.class,
        	HashMapUpdateFileSizeTestCase.class,
        	Iterator4JdkIteratorTestCase.class,
        	JdkCollectionIterator4TestCase.class,
        	KeepCollectionContentTestCase.class,
        	MixedCollectionTestCase.class,
        	NestedListTestCase.class,
        	PersistentListTestCase.class,
        	SetCollectionOnUpdateTestCase.class, 
        	TreeSetCustomComparableTestCase.class,
        	TreeSetTestSuite.class,
        	com.db4o.db4ounit.jre12.collections.map.AllTests.class, 
		};
    }
}
