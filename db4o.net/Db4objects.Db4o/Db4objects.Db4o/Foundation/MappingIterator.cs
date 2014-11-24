/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public abstract class MappingIterator : IEnumerator
	{
		private readonly IEnumerator _iterator;

		private object _current;

		public MappingIterator(IEnumerator iterator)
		{
			if (null == iterator)
			{
				throw new ArgumentNullException();
			}
			_iterator = iterator;
			_current = Iterators.NoElement;
		}

		protected abstract object Map(object current);

		public virtual bool MoveNext()
		{
			do
			{
				if (!_iterator.MoveNext())
				{
					_current = Iterators.NoElement;
					return false;
				}
				_current = Map(_iterator.Current);
			}
			while (_current == Iterators.Skip);
			return true;
		}

		public virtual void Reset()
		{
			_current = Iterators.NoElement;
			_iterator.Reset();
		}

		public virtual object Current
		{
			get
			{
				if (Iterators.NoElement == _current)
				{
					throw new InvalidOperationException();
				}
				return _current;
			}
		}
	}
}
