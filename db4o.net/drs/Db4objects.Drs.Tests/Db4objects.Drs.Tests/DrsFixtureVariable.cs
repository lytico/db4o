/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class DrsFixtureVariable
	{
		private static readonly FixtureVariable _variable = new FixtureVariable("drs");

		public static DrsFixture Value()
		{
			return (DrsFixture)_variable.Value;
		}

		public static object With(DrsFixture pair, IClosure4 closure)
		{
			return _variable.With(pair, closure);
		}
	}
}
