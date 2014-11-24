/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Extensions
{
	public class ObjectSetAssert
	{
		public static void SameContent(IObjectSet objectSet, object[] expectedItems)
		{
			Iterator4Assert.SameContent(Iterators.Iterate(expectedItems), Iterate(objectSet));
		}

		public static void AreEqual(IObjectSet objectSet, object[] expectedItems)
		{
			Iterator4Assert.AreEqual(expectedItems, Iterate(objectSet));
		}

		public static IEnumerator Iterate(IObjectSet objectSet)
		{
			return new ObjectSetAssert.ObjectSetIterator4(objectSet);
		}

		internal class ObjectSetIterator4 : IEnumerator
		{
			private static readonly object Invalid = new object();

			private IObjectSet _objectSet;

			private object _current;

			public ObjectSetIterator4(IObjectSet collection)
			{
				_objectSet = collection;
			}

			public virtual object Current
			{
				get
				{
					if (_current == Invalid)
					{
						throw new InvalidOperationException();
					}
					return _current;
				}
			}

			public virtual bool MoveNext()
			{
				if (_objectSet.HasNext())
				{
					_current = _objectSet.Next();
					return true;
				}
				_current = Invalid;
				return false;
			}

			public virtual void Reset()
			{
				_objectSet.Reset();
				_current = Invalid;
			}
		}
	}
}
