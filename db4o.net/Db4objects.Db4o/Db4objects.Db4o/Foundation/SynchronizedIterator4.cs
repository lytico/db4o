/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;

namespace Db4objects.Db4o.Foundation
{
	public class SynchronizedIterator4 : IEnumerator
	{
		private readonly IEnumerator _delegate;

		private readonly object _lock;

		public SynchronizedIterator4(IEnumerator delegate_, object Lock)
		{
			_delegate = delegate_;
			_lock = Lock;
		}

		public virtual object Current
		{
			get
			{
				lock (_lock)
				{
					return _delegate.Current;
				}
			}
		}

		public virtual bool MoveNext()
		{
			lock (_lock)
			{
				return _delegate.MoveNext();
			}
		}

		public virtual void Reset()
		{
			lock (_lock)
			{
				_delegate.Reset();
			}
		}
	}
}
