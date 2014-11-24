/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class IntIterators
	{
		public static IFixedSizeIntIterator4 ForInts(int[] array, int count)
		{
			return new IntIterator4Impl(array, count);
		}

		public static IIntIterator4 ForLongs(long[] ids)
		{
			return new _IIntIterator4_10(ids);
		}

		private sealed class _IIntIterator4_10 : IIntIterator4
		{
			public _IIntIterator4_10(long[] ids)
			{
				this.ids = ids;
				this._next = 0;
			}

			internal int _next;

			internal int _current;

			public int CurrentInt()
			{
				return this._current;
			}

			public object Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				if (this._next < ids.Length)
				{
					this._current = (int)ids[this._next];
					++this._next;
					return true;
				}
				return false;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}

			private readonly long[] ids;
		}
	}
}
