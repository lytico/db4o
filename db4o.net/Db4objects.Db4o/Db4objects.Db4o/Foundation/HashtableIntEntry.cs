/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class HashtableIntEntry : IEntry4, IDeepClone
	{
		public int _key;

		public object _object;

		public Db4objects.Db4o.Foundation.HashtableIntEntry _next;

		internal HashtableIntEntry(int key, object obj)
		{
			// FIELDS ARE PUBLIC SO THEY CAN BE REFLECTED ON IN JDKs <= 1.1
			_key = key;
			_object = obj;
		}

		public HashtableIntEntry()
		{
		}

		public virtual object Key()
		{
			return _key;
		}

		public virtual object Value()
		{
			return _object;
		}

		public virtual object DeepClone(object obj)
		{
			return DeepCloneInternal(new Db4objects.Db4o.Foundation.HashtableIntEntry(), obj);
		}

		public virtual bool SameKeyAs(Db4objects.Db4o.Foundation.HashtableIntEntry other)
		{
			return _key == other._key;
		}

		protected virtual Db4objects.Db4o.Foundation.HashtableIntEntry DeepCloneInternal(
			Db4objects.Db4o.Foundation.HashtableIntEntry entry, object obj)
		{
			entry._key = _key;
			entry._next = _next;
			if (_object is IDeepClone)
			{
				entry._object = ((IDeepClone)_object).DeepClone(obj);
			}
			else
			{
				entry._object = _object;
			}
			if (_next != null)
			{
				entry._next = (Db4objects.Db4o.Foundation.HashtableIntEntry)_next.DeepClone(obj);
			}
			return entry;
		}

		public override string ToString()
		{
			return string.Empty + _key + ": " + _object;
		}
	}
}
