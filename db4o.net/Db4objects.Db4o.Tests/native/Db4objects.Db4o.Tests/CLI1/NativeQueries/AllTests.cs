/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			{
				typeof(Cats.TestCatConsistency),
				typeof(Cat),
				typeof(DoubleNQTestCase),
				typeof(ListElementByIdentity),
#if !CF
				typeof(MultipleAssemblySupportTestCase),
#endif
				typeof(NativeQueriesTestCase),
				typeof(OptimizationFailuresTestCase),
				typeof(StringComparisonTestCase),
			};
		}
	}
}
