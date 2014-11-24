/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Fixtures;

namespace Db4oUnit.Fixtures
{
	public interface IFixtureProvider : IEnumerable
	{
		FixtureVariable Variable();
	}
}
