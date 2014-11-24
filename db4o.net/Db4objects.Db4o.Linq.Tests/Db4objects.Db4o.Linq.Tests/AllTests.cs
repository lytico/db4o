/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests
{
	public class AllTests : Db4oTestSuite
	{
		public static int Main(string[] args)
		{
			var res = new AllTests().RunAll();
			return res;
		}

		protected override Type[] TestCases()
		{
			return new [] {
				typeof(Caching.AllTests),
				typeof(CodeAnalysis.AllTests),
				typeof(Expressions.AllTests),
				typeof(Queries.AllTests),
				typeof(QueryOperators.AllTests),
                typeof(ByteQueryTestCase),
				typeof(CollectionContainsObjectTestCase),
				typeof(CollectionContainsOptimizationTestCase),
				typeof(CollectionContainsTestCase),
				typeof(ComposedQueryTestCase),
				typeof(CountTestCase),
				typeof(DateTimeOffsetQueryTestCase),
				typeof(EnumComparisonTestCase),
				typeof(NullConstantTestCase),
				typeof(OrderByTestCase),
				typeof(ParameterizedWhereTestCase),
				typeof(PartiallyOptimizedQueryTestCase),
				typeof(QueryableTestCase),
				typeof(QueryReuseTestCase),
				typeof(QueryTranslationPerformanceTestCase),
				typeof(StringMethodTestCase),
				typeof(UntypedQueryTestCase),
				typeof(VisualBasicTestCase),
				typeof(WhereTestCase),
#if !CF
				typeof(GenericQueryTestCase),
#endif
#if !CF && !SILVERLIGHT
				typeof(GenericQueryTestCase),
#endif
			};
		}
	}
}
