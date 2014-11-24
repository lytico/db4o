/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class IntIterator4Adaptor : IIntIterator4
	{
		private readonly IEnumerator _iterator;

		public IntIterator4Adaptor(IEnumerator iterator)
		{
			_iterator = iterator;
		}

		public IntIterator4Adaptor(IEnumerable iterable) : this(iterable.GetEnumerator())
		{
		}

		public virtual int CurrentInt()
		{
			return ((int)Current);
		}

		public virtual object Current
		{
			get
			{
				return _iterator.Current;
			}
		}

		public virtual bool MoveNext()
		{
			return _iterator.MoveNext();
		}

		public virtual void Reset()
		{
			_iterator.Reset();
		}
	}
}
