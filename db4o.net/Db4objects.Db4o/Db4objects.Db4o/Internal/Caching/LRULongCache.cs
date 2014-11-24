/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude></exclude>
	internal class LRULongCache : IPurgeableCache4
	{
		private class Entry
		{
			internal readonly long _key;

			internal readonly object _value;

			internal LRULongCache.Entry _previous;

			internal LRULongCache.Entry _next;

			public Entry(long key, object value)
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

		private LRULongCache.Entry _first;

		private LRULongCache.Entry _last;

		internal LRULongCache(int size)
		{
			_maxSize = size;
			_slots = new Hashtable4(size);
		}

		public virtual object Produce(object key, IFunction4 producer, IProcedure4 finalizer
			)
		{
			long longKey = (((long)key));
			if (_last == null)
			{
				object lastValue = producer.Apply(((long)key));
				if (lastValue == null)
				{
					return null;
				}
				_size = 1;
				LRULongCache.Entry lastEntry = new LRULongCache.Entry(longKey, lastValue);
				_slots.Put(longKey, lastEntry);
				_first = lastEntry;
				_last = lastEntry;
				return lastValue;
			}
			LRULongCache.Entry entry = (LRULongCache.Entry)_slots.Get(longKey);
			if (entry == null)
			{
				if (_size >= _maxSize)
				{
					LRULongCache.Entry oldEntry = (LRULongCache.Entry)_slots.Remove(_last._key);
					_last = oldEntry._previous;
					_last._next = null;
					if (null != finalizer)
					{
						finalizer.Apply((object)oldEntry._value);
					}
					_size--;
				}
				object newValue = producer.Apply(((long)key));
				if (newValue == null)
				{
					return null;
				}
				_size++;
				LRULongCache.Entry newEntry = new LRULongCache.Entry(longKey, newValue);
				_slots.Put(longKey, newEntry);
				_first._previous = newEntry;
				newEntry._next = _first;
				_first = newEntry;
				return newValue;
			}
			if (_first == entry)
			{
				return ((object)entry._value);
			}
			LRULongCache.Entry previous = entry._previous;
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
			public _IEnumerator_108(LRULongCache _enclosing)
			{
				this._enclosing = _enclosing;
				this._cursor = this._enclosing._first;
			}

			private LRULongCache.Entry _cursor;

			private LRULongCache.Entry _current;

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

			private readonly LRULongCache _enclosing;
		}

		public virtual object Purge(object key)
		{
			long longKey = (((long)key));
			LRULongCache.Entry entry = (LRULongCache.Entry)_slots.Remove(longKey);
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
