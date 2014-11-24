/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Tests.CLI2.Reflector;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Reflect
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[]
			       	{
			       		typeof(FastNetReflectorTestCase),
			       	};
		}
	}
}
