/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Extensions.Fixtures
{
	public class Db4oFixtureProvider : IFixtureProvider
	{
		public virtual FixtureVariable Variable()
		{
			return Db4oFixtureVariable.FixtureVariable;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return Iterators.SingletonIterator(Variable().Value);
		}
	}
}
