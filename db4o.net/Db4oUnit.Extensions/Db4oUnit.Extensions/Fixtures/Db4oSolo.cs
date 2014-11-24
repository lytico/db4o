/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oSolo : AbstractFileBasedDb4oFixture
	{
		private static readonly string File = "db4oSoloTest.db4o";

		public Db4oSolo()
		{
		}

		public Db4oSolo(IFixtureConfiguration fixtureConfiguration)
		{
			FixtureConfiguration(fixtureConfiguration);
		}

		public override string Label()
		{
			return BuildLabel("SOLO");
		}

		protected override string FileName()
		{
			return File;
		}
	}
}
