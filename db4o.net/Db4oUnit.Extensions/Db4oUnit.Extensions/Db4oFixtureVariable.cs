/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;

namespace Db4oUnit.Extensions
{
	public sealed class Db4oFixtureVariable
	{
		public static readonly FixtureVariable FixtureVariable = new FixtureVariable("db4o"
			);

		public static IDb4oFixture Fixture()
		{
			return (IDb4oFixture)FixtureVariable.Value;
		}

		private Db4oFixtureVariable()
		{
		}
	}
}
