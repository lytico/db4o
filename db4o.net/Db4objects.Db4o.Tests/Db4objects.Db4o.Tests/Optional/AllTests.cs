/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Optional;

namespace Db4objects.Db4o.Tests.Optional
{
	public class AllTests : ComposibleTestSuite
	{
		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(Db4objects.Db4o.Tests.Optional.Handlers.AllTests
				), typeof(ConsistencyCheckerTestSuite) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(FileUsageStatsTestCase) };
		}
		#endif // !SILVERLIGHT
	}
}
