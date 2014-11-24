/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class SynchronizedHashtable4 : IDeepClone
	{
		private readonly Hashtable4 _delegate;

		private SynchronizedHashtable4(Hashtable4 delegate_)
		{
			_delegate = delegate_;
		}

		public SynchronizedHashtable4(int size) : this(new Hashtable4(size))
		{
		}

		public virtual object DeepClone(object obj)
		{
			lock (this)
			{
				return new Db4objects.Db4o.Foundation.SynchronizedHashtable4((Hashtable4)_delegate
					.DeepClone(obj));
			}
		}

		public virtual void Put(object key, object value)
		{
			lock (this)
			{
				_delegate.Put(key, value);
			}
		}

		public virtual object Get(object key)
		{
			lock (this)
			{
				return _delegate.Get(key);
			}
		}
	}
}
