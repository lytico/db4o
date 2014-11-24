/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;

namespace Db4objects.Db4o.Internal.Caching
{
	/// <exclude>
	/// Full version of the algorithm taken from here:
	/// http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.34.2641
	/// </exclude>
	internal class LRU2QXCache : ICache4
	{
		private readonly IDictionary _slots;

		private readonly CircularBuffer4 _am;

		private readonly CircularBuffer4 _a1in;

		private readonly CircularBuffer4 _a1out;

		private readonly int _maxSize;

		private readonly int _inSize;

		public LRU2QXCache(int maxSize)
		{
			// 'eden': long-term lru queue
			// 'nursery': short-term fifo queue, entry point for all new items
			// 'backlog': fifo queue, elements may not be backed in _slots or may overlap with _am
			// invariant: |_slots| = |_am| + |_a1in| <= _maxSize
			_maxSize = maxSize;
			_inSize = _maxSize / 4;
			_slots = new Hashtable(_maxSize);
			_am = new CircularBuffer4(_maxSize);
			_a1in = new CircularBuffer4(_maxSize);
			_a1out = new CircularBuffer4(_maxSize / 2);
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
			if (_a1out.Contains(key))
			{
				ReclaimFor(key, producer, finalizer);
				_am.AddFirst(key);
				return _slots[key];
			}
			if (_a1in.Contains(key))
			{
				return _slots[key];
			}
			ReclaimFor(key, producer, finalizer);
			_a1in.AddFirst(key);
			return _slots[key];
		}

		private void ReclaimFor(object key, IFunction4 producer, IProcedure4 finalizer)
		{
			if (_slots.Count < _maxSize)
			{
				_slots[key] = producer.Apply(key);
				return;
			}
			if (_a1in.Size() > _inSize)
			{
				object lastKey = _a1in.RemoveLast();
				Discard(lastKey, finalizer);
				if (_a1out.IsFull())
				{
					_a1out.RemoveLast();
				}
				_a1out.AddFirst(lastKey);
			}
			else
			{
				object lastKey = _am.RemoveLast();
				Discard(lastKey, finalizer);
			}
			_slots[key] = producer.Apply(key);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return _slots.Values.GetEnumerator();
		}

		public override string ToString()
		{
			return "LRU2QXCache(am=" + ToString(_am) + ", a1in=" + ToString(_a1in) + ", a1out="
				 + ToString(_a1out) + ")" + " - " + _slots.Count;
		}

		private void Discard(object key, IProcedure4 finalizer)
		{
			object removed = Sharpen.Collections.Remove(_slots, key);
			if (finalizer != null)
			{
				finalizer.Apply(removed);
			}
		}

		private string ToString(IEnumerable buffer)
		{
			return Iterators.ToString(buffer);
		}
	}
}
