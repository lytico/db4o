/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Fixtures
{
	public class MultiValueFixtureProvider : IFixtureProvider
	{
		public static object[] Value()
		{
			return (object[])_variable.Value;
		}

		private static readonly FixtureVariable _variable = new FixtureVariable("data");

		private readonly object[][] _values;

		public MultiValueFixtureProvider(object[][] values)
		{
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
