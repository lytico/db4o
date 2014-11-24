/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude>
	/// Simplified version of the algorithm taken from here:
	/// http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.34.2641
	/// </exclude>
	internal class LRU2QCache : ICache4
	{
		private readonly CircularBuffer4 _am;

		private readonly CircularBuffer4 _a1;

		private readonly IDictionary _slots;

		private readonly int _maxSize;

		private readonly int _a1_threshold;

		internal LRU2QCache(int maxSize)
		{
			_maxSize = maxSize;
			_a1_threshold = _maxSize / 4;
			_am = new CircularBuffer4(_maxSize);
			_a1 = new CircularBuffer4(_maxSize);
			_slots = new Hashtable(maxSize);
		}

		public virtual object Produce(object key, IFunction4 producer, IProcedure4 finalizer
			)
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			if (_am.Remove(key))
			{
				_am.AddFirst(key);
				return _slots[key];
			}
			if (_a1.Remove(key))
			{
				_am.AddFirst(key);
				return _slots[key];
			}
			if (_slots.Count >= _maxSize)
			{
				DiscardPage(finalizer);
			}
			object value = producer.Apply(key);
			_slots[key] = value;
			_a1.AddFirst(key);
			return value;
		}

		private void DiscardPage(IProcedure4 finalizer)
		{
			if (_a1.Size() >= _a1_threshold)
			{
				DiscardPageFrom(_a1, finalizer);
			}
			else
			{
				DiscardPageFrom(_am, finalizer);
			}
		}

		private void DiscardPageFrom(CircularBuffer4 list, IProcedure4 finalizer)
		{
			Discard(list.RemoveLast(), finalizer);
		}

		private void Discard(object key, IProcedure4 finalizer)
		{
			if (null != finalizer)
			{
				finalizer.Apply(_slots[key]);
			}
			Sharpen.Collections.Remove(_slots, key);
		}

		public override string ToString()
		{
			return "LRU2QCache(am=" + ToString(_am) + ", a1=" + ToString(_a1) + ")";
		}

		private string ToString(IEnumerable buffer)
		{
			return Iterators.ToString(buffer);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _slots.Values.GetEnumerator();
		}
	}
}
