/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Iterator4Impl : IEnumerator
	{
		private readonly List4 _first;

		private List4 _next;

		private object _current;

		public Iterator4Impl(List4 first)
		{
			_first = first;
			_next = first;
			_current = Iterators.NoElement;
		}

		public virtual bool MoveNext()
		{
			if (_next == null)
			{
				_current = Iterators.NoElement;
				return false;
			}
			_current = ((object)_next._element);
			_next = ((List4)_next._next);
			return true;
		}

		public virtual object Current
		{
			get
			{
				if (Iterators.NoElement == _current)
				{
					throw new InvalidOperationException();
				}
				return (object)_current;
			}
		}

		public virtual void Reset()
		{
			_next = _first;
			_current = Iterators.NoElement;
		}
	}
}
