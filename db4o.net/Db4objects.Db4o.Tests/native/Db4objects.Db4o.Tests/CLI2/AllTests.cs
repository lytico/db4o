/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			{
				typeof(Assorted.AllTests),
				typeof(Collections.AllTests),
				typeof(Defrag.AllTests),
                typeof(Handlers.AllTests),
				typeof(NativeQueries.Diagnostics.AllTests),
				typeof(Regression.AllTests),
				typeof(Refactor.AllTests),
				typeof(Reflect.AllTests),
				typeof(TA.AllTests),
				typeof(Types.AllTests),
			};
		}
	}
}
