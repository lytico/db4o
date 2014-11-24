/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	internal class LRUIntCache : IPurgeableCache4
	{
		private class Entry
		{
			internal readonly int _key;

			internal readonly object _value;

			internal LRUIntCache.Entry _previous;

			internal LRUIntCache.Entry _next;

			public Entry(int key, object value)
			{
				_key = key;
				_value = value;
			}

			public override string ToString()
			{
				return string.Empty + _key;
			}
		}

		private readonly Hashtable4 _slots;

		private readonly int _maxSize;

		private int _size;

		private LRUIntCache.Entry _first;

		private LRUIntCache.Entry _last;

		internal LRUIntCache(int size)
		{
			_maxSize = size;
			_slots = new Hashtable4(size);
		}

		public virtual object Produce(object key, IFunction4 producer, IProcedure4 finalizer
			)
		{
			int intKey = (((int)key));
			if (_last == null)
			{
				object lastValue = producer.Apply(((int)key));
				if (lastValue == null)
				{
					return null;
				}
				_size = 1;
				LRUIntCache.Entry lastEntry = new LRUIntCache.Entry(intKey, lastValue);
				_slots.Put(intKey, lastEntry);
				_first = lastEntry;
				_last = lastEntry;
				return lastValue;
			}
			LRUIntCache.Entry entry = (LRUIntCache.Entry)_slots.Get(intKey);
			if (entry == null)
			{
				if (_size >= _maxSize)
				{
					LRUIntCache.Entry oldEntry = (LRUIntCache.Entry)_slots.Remove(_last._key);
					_last = oldEntry._previous;
					_last._next = null;
					if (null != finalizer)
					{
						finalizer.Apply((object)oldEntry._value);
					}
					_size--;
				}
				object newValue = producer.Apply(((int)key));
				if (newValue == null)
				{
					return null;
				}
				_size++;
				LRUIntCache.Entry newEntry = new LRUIntCache.Entry(intKey, newValue);
				_slots.Put(intKey, newEntry);
				_first._previous = newEntry;
				newEntry._next = _first;
				_first = newEntry;
				return newValue;
			}
			if (_first == entry)
			{
				return ((object)entry._value);
			}
			LRUIntCache.Entry previous = entry._previous;
			entry._previous = null;
			if (_last == entry)
			{
				_last = previous;
			}
			previous._next = entry._next;
			if (previous._next != null)
			{
				previous._next._previous = previous;
			}
			_first._previous = entry;
			entry._next = _first;
			_first = entry;
			return ((object)entry._value);
		}

		public virtual IEnumerator GetEnumerator()
		{
			IEnumerator i = new _IEnumerator_108(this);
			return i;
		}

		private sealed class _IEnumerator_108 : IEnumerator
		{
			public _IEnumerator_108(LRUIntCache _enclosing)
			{
				this._enclosing = _enclosing;
				this._cursor = this._enclosing._first;
			}

			private LRUIntCache.Entry _cursor;

			private LRUIntCache.Entry _current;

			public object Current
			{
				get
				{
					return this._current._value;
				}
			}

			public bool MoveNext()
			{
				if (this._cursor == null)
				{
					this._current = null;
					return false;
				}
				this._current = this._cursor;
				this._cursor = this._cursor._next;
				return true;
			}

			public void Reset()
			{
				this._cursor = this._enclosing._first;
				this._current = null;
			}

			private readonly LRUIntCache _enclosing;
		}

		public virtual object Purge(object key)
		{
			int intKey = (((int)key));
			LRUIntCache.Entry entry = (LRUIntCache.Entry)_slots.Remove(intKey);
			if (entry == null)
			{
				return null;
			}
			_size--;
			if (_first == entry)
			{
				_first = entry._next;
			}
			if (_last == entry)
			{
				_last = entry._previous;
			}
			if (entry._previous != null)
			{
				entry._previous._next = entry._next;
			}
			if (entry._next != null)
			{
				entry._next._previous = entry._previous;
			}
			return ((object)entry._value);
		}
	}
}
