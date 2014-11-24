/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests.QueryOperators
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new[]
			       	{
			       		typeof(SkipTestSuite),
						typeof(SkipTestCase),
			       	};
		}
	}
}
