/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Db4objects.Db4o.Linq.Internals
{
	/// <summary>
	/// A generic wrapper around a not generic IEnumerable,
	/// Faithfully hoping that all items in the enumeration
	/// are of the same kind, otherwise it will throw a
	/// ClassCastException on access.
	/// </summary>
	/// <typeparam name="T">The type of the items</typeparam>
	public class ObjectSequence<T> : IEnumerable<T>
	{
		private IEnumerable _enumerable;

		public ObjectSequence(IEnumerable enumerable)
		{
			_enumerable = enumerable;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new ObjectSequenceEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal class ObjectSequenceEnumerator : IEnumerator<T>
		{
			private IEnumerator _enumerator;

			public T Current {
				get { return (T)_enumerator.Current; }
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public ObjectSequenceEnumerator(ObjectSequence<T> sequence)
			{
				_enumerator = sequence._enumerable.GetEnumerator();
			}

			public bool MoveNext()
			{
				return _enumerator.MoveNext();
			}

			public void Reset()
			{
				_enumerator.Reset();
			}

			public void Dispose()
			{
				IDisposable enumerator = _enumerator as IDisposable;
				if (enumerator == null) return;

				enumerator.Dispose();
			}
		}
	}
}
