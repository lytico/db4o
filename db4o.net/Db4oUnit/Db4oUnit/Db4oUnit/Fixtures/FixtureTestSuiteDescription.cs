/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit.Fixtures;

namespace Db4oUnit.Fixtures
{
	public class FixtureTestSuiteDescription : FixtureBasedTestSuite
	{
		private IFixtureProvider[] _providers;

		private Type[] _testUnits;

		public virtual void FixtureProviders(IFixtureProvider[] providers)
		{
			_providers = providers;
		}

		public virtual void TestUnits(Type[] testUnits)
		{
			_testUnits = testUnits;
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return _providers;
		}

		public override Type[] TestUnits()
		{
			return _testUnits;
		}
	}
}
