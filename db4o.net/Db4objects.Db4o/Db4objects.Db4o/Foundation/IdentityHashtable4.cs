/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Foundation
{
	public class IdentityHashtable4 : HashtableBase, IMap4
	{
		public IdentityHashtable4()
		{
		}

		public IdentityHashtable4(int size) : base(size)
		{
		}

		public virtual bool Contains(object obj)
		{
			return GetEntry(obj) != null;
		}

		public virtual object Remove(object obj)
		{
			if (null == obj)
			{
				throw new ArgumentNullException();
			}
			return RemoveIntEntry(Runtime.IdentityHashCode(obj));
		}

		public virtual bool ContainsKey(object key)
		{
			return GetEntry(key) != null;
		}

		public virtual object Get(object key)
		{
			HashtableIntEntry entry = GetEntry(key);
			return (entry == null ? null : entry._object);
		}

		private HashtableIntEntry GetEntry(object key)
		{
			return FindWithSameKey(new IdentityHashtable4.IdentityEntry(key));
		}

		public virtual void Put(object key, object value)
		{
			if (null == key)
			{
				throw new ArgumentNullException();
			}
			PutEntry(new IdentityHashtable4.IdentityEntry(key, value));
		}

		public class IdentityEntry : HashtableObjectEntry
		{
			public IdentityEntry(object obj) : this(obj, null)
			{
			}

			public IdentityEntry(object key, object value) : base(Runtime.IdentityHashCode(key
				), key, value)
			{
			}

			public override bool HasKey(object key)
			{
				return _objectKey == key;
			}
		}
	}
}
