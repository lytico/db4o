/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class Collections4
	{
		public static ISequence4 UnmodifiableList(ISequence4 orig)
		{
			return new Collections4.UnmodifiableSequence4(orig);
		}

		public static void Sort(ISequence4 sequence, IComparison4 comparator)
		{
			object[] array = sequence.ToArray();
			Arrays4.Sort(array, comparator);
			sequence.Clear();
			for (int oIndex = 0; oIndex < array.Length; ++oIndex)
			{
				object o = array[oIndex];
				sequence.Add(o);
			}
		}

		private class UnmodifiableSequence4 : ISequence4
		{
			private ISequence4 _sequence;

			public UnmodifiableSequence4(ISequence4 sequence)
			{
				_sequence = sequence;
			}

			public virtual bool Add(object element)
			{
				throw new NotSupportedException();
			}

			public virtual void AddAll(IEnumerable iterable)
			{
				throw new NotSupportedException();
			}

			public virtual bool IsEmpty()
			{
				return _sequence.IsEmpty();
			}

			public virtual IEnumerator GetEnumerator()
			{
				return _sequence.GetEnumerator();
			}

			public virtual object Get(int index)
			{
				return _sequence.Get(index);
			}

			public virtual int Size()
			{
				return _sequence.Size();
			}

			public virtual void Clear()
			{
				throw new NotSupportedException();
			}

			public virtual bool Remove(object obj)
			{
				throw new NotSupportedException();
			}

			public virtual bool Contains(object obj)
			{
				return _sequence.Contains(obj);
			}

			public virtual bool ContainsAll(IEnumerable iter)
			{
				return _sequence.ContainsAll(iter);
			}

			public virtual object[] ToArray()
			{
				return _sequence.ToArray();
			}

			public virtual object[] ToArray(object[] array)
			{
				return _sequence.ToArray(array);
			}
		}
	}
}
