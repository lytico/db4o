/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions.Tests;

namespace Db4oUnit.Extensions.Tests
{
	public class AllTests : ReflectionTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4oUnit.Extensions.Tests.AllTests().Run();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(Db4oEmbeddedSessionFixtureTestCase), typeof(DynamicFixtureTestCase
				), typeof(ExcludingReflectorTestCase), typeof(FixtureConfigurationTestCase), typeof(
				FixtureTestCase), typeof(UnhandledExceptionInThreadTestCase) };
		}
	}
}
