/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class AllTests : ComposibleTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Querying.AllTests().RunAll();
		}

		//runSoloAndClientServer();
		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(CascadedDeleteUpdate), typeof(CascadeDeleteArray
				), typeof(CascadeDeleteDeleted), typeof(CascadeDeleteFalse), typeof(CascadeOnActivate
				), typeof(CascadeOnDeleteTestCase), typeof(CascadeOnDeleteHierarchyTestCase), typeof(
				CascadeOnUpdateTestCase), typeof(CascadeToArray), typeof(ConjunctiveQbETestCase)
				, typeof(DeepMultifieldSortingTestCase), typeof(DescendIndexQueryTestCase), typeof(
				IdentityQueryForNotStoredTestCase), typeof(IdListQueryResultTestCase), typeof(IndexedJoinQueriesTestCase
				), typeof(IndexOnParentFieldTestCase), typeof(IndexedQueriesTestCase), typeof(InvalidFieldNameConstraintTestCase
				), typeof(LazyQueryResultTestCase), typeof(MultiFieldIndexQueryTestCase), typeof(
				NoClassIndexQueryTestSuite), typeof(NullConstraintQueryTestCase), typeof(ObjectSetTestCase
				), typeof(OrderedQueryTestCase), typeof(QueryByExampleTestCase), typeof(QueryingForAllObjectsTestCase
				), typeof(QueryingVersionFieldTestCase), typeof(SameChildOnDifferentParentQueryTestCase
				), typeof(TwoLevelIndexTestCase) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(NonPrimitiveSortingTestCase) };
		}
		#endif // !SILVERLIGHT
	}
}
