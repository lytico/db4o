/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class Hashtable4 : HashtableBase, IDeepClone, IMap4
	{
		public Hashtable4(int size) : base(size)
		{
		}

		public Hashtable4() : this(1)
		{
		}

		/// <param name="cloneOnlyCtor"></param>
		protected Hashtable4(IDeepClone cloneOnlyCtor) : base(cloneOnlyCtor)
		{
		}

		public virtual object DeepClone(object obj)
		{
			return DeepCloneInternal(new Db4objects.Db4o.Foundation.Hashtable4((IDeepClone)null
				), obj);
		}

		public virtual void ForEachKeyForIdentity(IVisitor4 visitor, object obj)
		{
			for (int i = 0; i < _table.Length; i++)
			{
				HashtableIntEntry entry = _table[i];
				while (entry != null)
				{
					if (entry._object == obj)
					{
						visitor.Visit(entry.Key());
					}
					entry = entry._next;
				}
			}
		}

		public virtual object Get(byte[] key)
		{
			int intKey = HashtableByteArrayEntry.Hash(key);
			return GetFromObjectEntry(intKey, key);
		}

		public virtual object Get(int key)
		{
			HashtableIntEntry entry = _table[key & _mask];
			while (entry != null)
			{
				if (entry._key == key)
				{
					return entry._object;
				}
				entry = entry._next;
			}
			return null;
		}

		public virtual object Get(object key)
		{
			if (key == null)
			{
				return null;
			}
			return GetFromObjectEntry(key.GetHashCode(), key);
		}

		public virtual object Get(long key)
		{
			return GetFromLongEntry((int)key, key);
		}

		public virtual bool ContainsKey(object key)
		{
			if (null == key)
			{
				return false;
			}
			return null != GetObjectEntry(key.GetHashCode(), key);
		}

		public virtual bool ContainsAllKeys(IEnumerable collection)
		{
			return ContainsAllKeys(collection.GetEnumerator());
		}

		public virtual bool ContainsAllKeys(IEnumerator iterator)
		{
			while (iterator.MoveNext())
			{
				if (!ContainsKey(iterator.Current))
				{
					return false;
				}
			}
			return true;
		}

		public virtual void Put(byte[] key, object value)
		{
			PutEntry(new HashtableByteArrayEntry(key, value));
		}

		public virtual void Put(int key, object value)
		{
			PutEntry(new HashtableIntEntry(key, value));
		}

		public virtual void Put(long key, object value)
		{
			PutEntry(new HashtableLongEntry(key, value));
		}

		public virtual void Put(object key, object value)
		{
			if (null == key)
			{
				throw new ArgumentNullException();
			}
			PutEntry(new HashtableObjectEntry(key, value));
		}

		public virtual object Remove(object objectKey)
		{
			int intKey = objectKey.GetHashCode();
			return RemoveObjectEntry(intKey, objectKey);
		}

		public virtual object Remove(long longKey)
		{
			return RemoveLongEntry((int)longKey, longKey);
		}

		public virtual object Remove(byte[] key)
		{
			int intKey = HashtableByteArrayEntry.Hash(key);
			return RemoveObjectEntry(intKey, key);
		}

		public virtual object Remove(int key)
		{
			return RemoveIntEntry(key);
		}

		/// <summary>
		/// Iterates through all the
		/// <see cref="IEntry4">entries</see>
		/// .
		/// </summary>
		/// <returns>
		/// 
		/// <see cref="IEntry4">IEntry4</see>
		/// iterator
		/// </returns>
		/// <seealso cref="HashtableBase.Values()">HashtableBase.Values()</seealso>
		/// <seealso cref="HashtableBase.Keys()">
		/// #see
		/// <see cref="HashtableBase.ValuesIterator()">HashtableBase.ValuesIterator()</see>
		/// </seealso>
		public virtual IEnumerator Iterator()
		{
			return HashtableIterator();
		}

		protected virtual Db4objects.Db4o.Foundation.Hashtable4 DeepCloneInternal(Db4objects.Db4o.Foundation.Hashtable4
			 ret, object obj)
		{
			ret._mask = _mask;
			ret._maximumSize = _maximumSize;
			ret._size = _size;
			ret._tableSize = _tableSize;
			ret._table = new HashtableIntEntry[_tableSize];
			for (int i = 0; i < _tableSize; i++)
			{
				if (_table[i] != null)
				{
					ret._table[i] = (HashtableIntEntry)_table[i].DeepClone(obj);
				}
			}
			return ret;
		}

		private object GetFromObjectEntry(int intKey, object objectKey)
		{
			HashtableObjectEntry entry = GetObjectEntry(intKey, objectKey);
			return entry == null ? null : entry._object;
		}

		private HashtableObjectEntry GetObjectEntry(int intKey, object objectKey)
		{
			HashtableObjectEntry entry = (HashtableObjectEntry)_table[intKey & _mask];
			while (entry != null)
			{
				if (entry._key == intKey && entry.HasKey(objectKey))
				{
					return entry;
				}
				entry = (HashtableObjectEntry)entry._next;
			}
			return null;
		}

		private object GetFromLongEntry(int intKey, long longKey)
		{
			HashtableLongEntry entry = GetLongEntry(intKey, longKey);
			return entry == null ? null : entry._object;
		}

		private HashtableLongEntry GetLongEntry(int intKey, long longKey)
		{
			HashtableLongEntry entry = (HashtableLongEntry)_table[intKey & _mask];
			while (entry != null)
			{
				if (entry._key == intKey && entry._longKey == longKey)
				{
					return entry;
				}
				entry = (HashtableLongEntry)entry._next;
			}
			return null;
		}
	}
}
