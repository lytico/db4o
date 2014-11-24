/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class KeySpecHashtable4 : IDeepClone
	{
		private SynchronizedHashtable4 _delegate;

		private KeySpecHashtable4(SynchronizedHashtable4 delegate_)
		{
			_delegate = delegate_;
		}

		public KeySpecHashtable4(int size) : this(new SynchronizedHashtable4(size))
		{
		}

		public virtual void Put(KeySpec spec, byte value)
		{
			_delegate.Put(spec, value);
		}

		public virtual void Put(KeySpec spec, bool value)
		{
			_delegate.Put(spec, value);
		}

		public virtual void Put(KeySpec spec, int value)
		{
			_delegate.Put(spec, value);
		}

		public virtual void Put(KeySpec spec, object value)
		{
			_delegate.Put(spec, value);
		}

		public virtual byte GetAsByte(KeySpec spec)
		{
			return ((byte)Get(spec));
		}

		public virtual bool GetAsBoolean(KeySpec spec)
		{
			return ((bool)Get(spec));
		}

		public virtual int GetAsInt(KeySpec spec)
		{
			return ((int)Get(spec));
		}

		public virtual long GetAsLong(KeySpec spec)
		{
			return ((long)Get(spec));
		}

		public virtual TernaryBool GetAsTernaryBool(KeySpec spec)
		{
			return (TernaryBool)Get(spec);
		}

		public virtual string GetAsString(KeySpec spec)
		{
			return (string)Get(spec);
		}

		public virtual object Get(KeySpec spec)
		{
			lock (this)
			{
				object value = _delegate.Get(spec);
				if (value == null)
				{
					value = spec.DefaultValue();
					if (value != null)
					{
						_delegate.Put(spec, value);
					}
				}
				return value;
			}
		}

		public virtual object DeepClone(object obj)
		{
			return new Db4objects.Db4o.Foundation.KeySpecHashtable4((SynchronizedHashtable4)_delegate
				.DeepClone(obj));
		}
	}
}
