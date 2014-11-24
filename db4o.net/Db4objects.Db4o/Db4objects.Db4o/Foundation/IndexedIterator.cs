/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// Basic functionality for implementing iterators for
	/// fixed length structures whose elements can be efficiently
	/// accessed by a numeric index.
	/// </summary>
	/// <remarks>
	/// Basic functionality for implementing iterators for
	/// fixed length structures whose elements can be efficiently
	/// accessed by a numeric index.
	/// </remarks>
	public abstract class IndexedIterator : IEnumerator
	{
		private readonly int _length;

		private int _next;

		public IndexedIterator(int length)
		{
			_length = length;
			_next = -1;
		}

		public virtual bool MoveNext()
		{
			if (_next < LastIndex())
			{
				++_next;
				return true;
			}
			// force exception on unexpected call to current
			_next = _length;
			return false;
		}

		public virtual object Current
		{
			get
			{
				return Get(_next);
			}
		}

		public virtual void Reset()
		{
			_next = -1;
		}

		protected abstract object Get(int index);

		private int LastIndex()
		{
			return _length - 1;
		}
	}
}
