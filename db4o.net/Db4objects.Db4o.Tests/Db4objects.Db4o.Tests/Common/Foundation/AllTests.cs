/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class AllTests : ReflectionTestSuite
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Db4objects.Db4o.Tests.Common.Foundation.AllTests)).Run
				();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ArrayIterator4TestCase), typeof(Arrays4TestCase), typeof(
				BitMap4TestCase), typeof(BlockingQueueTestCase), typeof(PausableBlockingQueueTestCase
				), typeof(BufferTestCase), typeof(CircularBufferTestCase), typeof(Collection4TestCase
				), typeof(CompositeIterator4TestCase), typeof(Runtime4TestCase), typeof(DynamicVariableTestCase
				), typeof(EnvironmentsTestCase), typeof(HashSet4TestCase), typeof(Hashtable4TestCase
				), typeof(IdentityHashtable4TestCase), typeof(IdentitySet4TestCase), typeof(IntArrayListTestCase
				), typeof(IntMatcherTestCase), typeof(Iterable4AdaptorTestCase), typeof(IteratorsTestCase
				), typeof(Map4TestCase), typeof(NoDuplicatesQueueTestCase), typeof(NonblockingQueueTestCase
				), typeof(ObjectPoolTestCase), typeof(Path4TestCase), typeof(SortedCollection4TestCase
				), typeof(Stack4TestCase), typeof(TimeStampIdGeneratorTestCase), typeof(TreeKeyIteratorTestCase
				), typeof(TreeNodeIteratorTestCase), typeof(TreeTestCase) };
		}
	}
}
