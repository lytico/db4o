/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Tests.Fixtures;

namespace Db4oUnit.Tests.Fixtures
{
	public class AllTests : ReflectionTestSuite
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Db4oUnit.Tests.Fixtures.AllTests)).Run();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(FixtureBasedTestSuiteTestCase), typeof(FixtureContextTestCase
				), typeof(Set4TestSuite) };
		}
	}
}
