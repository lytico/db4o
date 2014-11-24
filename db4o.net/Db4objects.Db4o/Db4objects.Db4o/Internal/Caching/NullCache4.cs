/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	public class NullCache4 : ICache4
	{
		public virtual object Produce(object key, IFunction4 producer, IProcedure4 onDiscard
			)
		{
			return producer.Apply(key);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return Iterators.EmptyIterator;
		}
	}
}
