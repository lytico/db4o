/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			       	{
			       		typeof(Dictionary.AllTests),
			       		typeof(List.AllTests),
					};
		}
	}
}
