/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Soda
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new []
			       	{
						typeof(CoerceUnsignedTypesTestSuite),
			       		typeof(STValueTypeOrderByTestSuite),
			       	};
		}
	}
}
