/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Fixtures
{
	public class SimpleFixtureProvider : IFixtureProvider
	{
		private readonly FixtureVariable _variable;

		private readonly object[] _values;

		public SimpleFixtureProvider(FixtureVariable variable, object[] values)
		{
			_variable = variable;
			_values = values;
		}

		public virtual FixtureVariable Variable()
		{
			return _variable;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return Iterators.Iterate(_values);
		}
	}
}
