/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	public abstract class AbstractBTreeRangeIterator : IEnumerator
	{
		private readonly BTreeRangeSingle _range;

		private BTreePointer _cursor;

		private BTreePointer _current;

		public AbstractBTreeRangeIterator(BTreeRangeSingle range)
		{
			_range = range;
			BTreePointer first = range.First();
			if (first != null)
			{
				// we clone here, because we are calling unsafeNext() on BTreePointer
				// _cursor is our private copy, we modify it and never pass it out.
				_cursor = first.ShallowClone();
			}
		}

		public virtual bool MoveNext()
		{
			if (ReachedEnd())
			{
				_current = null;
				return false;
			}
			if (_current == null)
			{
				_current = _cursor.ShallowClone();
			}
			else
			{
				_cursor.CopyTo(_current);
			}
			_cursor = _cursor.UnsafeFastNext();
			return true;
		}

		public virtual void Reset()
		{
			_cursor = _range.First();
		}

		protected virtual BTreePointer CurrentPointer()
		{
			if (null == _current)
			{
				throw new InvalidOperationException();
			}
			return _current;
		}

		private bool ReachedEnd()
		{
			if (_cursor == null)
			{
				return true;
			}
			if (_range.End() == null)
			{
				return false;
			}
			return _range.End().Equals(_cursor);
		}

		public abstract object Current
		{
			get;
		}
	}
}
