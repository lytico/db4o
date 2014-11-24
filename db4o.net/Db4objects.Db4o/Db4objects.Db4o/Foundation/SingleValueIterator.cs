/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class SingleValueIterator : IEnumerator
	{
		private object _value;

		private bool _moved;

		public SingleValueIterator(object value)
		{
			_value = value;
		}

		public virtual object Current
		{
			get
			{
				if (!_moved || _value == Iterators.NoElement)
				{
					throw new InvalidOperationException();
				}
				return _value;
			}
		}

		public virtual bool MoveNext()
		{
			if (!_moved)
			{
				_moved = true;
				return true;
			}
			_value = Iterators.NoElement;
			return false;
		}

		public virtual void Reset()
		{
			throw new NotImplementedException();
		}
	}
}
