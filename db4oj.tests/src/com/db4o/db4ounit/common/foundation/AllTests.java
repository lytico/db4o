/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;


import db4ounit.*;


public class AllTests extends ReflectionTestSuite {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(AllTests.class).run();
	}

	@Override
	protected Class[] testCases() {
		return new Class[] {
			Algorithms4TestCase.class,
			ArrayIterator4TestCase.class,
			Arrays4TestCase.class,
			BitMap4TestCase.class,
			BlockingQueueTestCase.class,
			PausableBlockingQueueTestCase.class,
			BufferTestCase.class,
			CircularBufferTestCase.class,
			Collection4TestCase.class,
			Collections4TestCase.class,
			CompositeIterator4TestCase.class,
			Runtime4TestCase.class,
			DynamicVariableTestCase.class,
			EnvironmentsTestCase.class,
			HashSet4TestCase.class,
			Hashtable4TestCase.class,
			IdentityHashtable4TestCase.class,
			IdentitySet4TestCase.class,
			IntArrayListTestCase.class,
			IntMatcherTestCase.class,
			Iterable4AdaptorTestCase.class,
			IteratorsTestCase.class,
			Map4TestCase.class,
			NoDuplicatesQueueTestCase.class,
			NonblockingQueueTestCase.class,
			ObjectPoolTestCase.class,
			Path4TestCase.class,
			SortedCollection4TestCase.class,
			Stack4TestCase.class,
			TimeStampIdGeneratorTestCase.class,
			TreeKeyIteratorTestCase.class,
			TreeNodeIteratorTestCase.class,
			TreeTestCase.class,
		};
	}

}
