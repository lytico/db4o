using System;
using Db4objects.Db4o.Tests.Jre5.Concurrency.Query;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests
{
	class AllTestsConcurrency : Db4oConcurrencyTestSuite
	{
		protected override System.Type[] TestCases()
		{
			return new Type[]
			{
#if !SILVERLIGHT
				typeof(Db4objects.Db4o.Tests.Common.Concurrency.AllTests),
#endif
				typeof(ConcurrentLazyQueriesTestCase),
			};
		}
	}
}
