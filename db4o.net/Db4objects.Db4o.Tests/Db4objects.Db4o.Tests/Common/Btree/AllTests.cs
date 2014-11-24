/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Btree.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(BTreeAddRemoveTestCase), typeof(BTreeAsSetTestCase), typeof(
				BTreeFreeTestCase), typeof(BTreeIteratorTestCase), typeof(BTreeNodeTestCase), typeof(
				BTreePointerTestCase), typeof(BTreeRangeTestCase), typeof(BTreeRollbackTestCase)
				, typeof(BTreeSearchTestCase), typeof(BTreeSimpleTestCase), typeof(BTreeStructureChangeListenerTestCase
				), typeof(SearcherLowestHighestTestCase), typeof(SearcherTestCase) };
		}
	}
}
