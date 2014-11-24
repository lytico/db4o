/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class CompositeIterable4 : IEnumerable
	{
		private readonly IEnumerable _iterables;

		public CompositeIterable4(IEnumerable iterables)
		{
			_iterables = iterables;
		}

		public virtual IEnumerator GetEnumerator()
		{
			return new _CompositeIterator4_15(_iterables.GetEnumerator());
		}

		private sealed class _CompositeIterator4_15 : CompositeIterator4
		{
			public _CompositeIterator4_15(IEnumerator baseArg1) : base(baseArg1)
			{
			}

			protected override IEnumerator NextIterator(object current)
			{
				return ((IEnumerable)current).GetEnumerator();
			}
		}
	}
}
