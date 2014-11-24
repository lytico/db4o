/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public abstract class FixedSizeIntIterator4Base : IFixedSizeIntIterator4
	{
		private readonly int _size;

		private int _current;

		private int _available;

		public FixedSizeIntIterator4Base(int size)
		{
			this._size = size;
			_available = size;
		}

		public virtual int Size()
		{
			return _size;
		}

		public virtual int CurrentInt()
		{
			return _current;
		}

		public virtual object Current
		{
			get
			{
				return _current;
			}
		}

		public virtual bool MoveNext()
		{
			if (_available > 0)
			{
				--_available;
				_current = NextInt();
				return true;
			}
			return false;
		}

		protected abstract int NextInt();

		public virtual void Reset()
		{
			throw new NotImplementedException();
		}
	}
}
