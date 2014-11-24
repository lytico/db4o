/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit
{
	public class Iterator4Assert
	{
		public static void AreEqual(IEnumerable expected, IEnumerable actual)
		{
			AreEqual(expected.GetEnumerator(), actual.GetEnumerator());
		}

		public static void AreEqual(IEnumerator expected, IEnumerator actual)
		{
			if (null == expected)
			{
				Assert.IsNull(actual);
				return;
			}
			Assert.IsNotNull(actual);
			while (expected.MoveNext())
			{
				AssertNext(expected.Current, actual);
			}
			if (actual.MoveNext())
			{
				Unexpected(actual.Current);
			}
		}

		private static void Unexpected(object element)
		{
			Assert.Fail("Unexpected element: " + element);
		}

		public static void AssertNext(object expected, IEnumerator iterator)
		{
			Assert.IsTrue(iterator.MoveNext(), "'" + expected + "' expected.");
			Assert.AreEqual(expected, iterator.Current);
		}

		public static void AreEqual(object[] expected, IEnumerator iterator)
		{
			AreEqual(new ArrayIterator4(expected), iterator);
		}

		public static void SameContent(object[] expected, IEnumerator actual)
		{
			SameContent(new ArrayIterator4(expected), actual);
		}

		public static void SameContent(IEnumerator expected, IEnumerator actual)
		{
			Collection4 allExpected = new Collection4(expected);
			while (actual.MoveNext())
			{
				object current = actual.Current;
				bool removed = allExpected.Remove(current);
				if (!removed)
				{
					Unexpected(current);
				}
			}
			Assert.IsTrue(allExpected.IsEmpty(), "Still missing: " + allExpected.ToString());
		}

		public static void AreInstanceOf(Type expectedType, IEnumerable values)
		{
			for (IEnumerator i = values.GetEnumerator(); i.MoveNext(); )
			{
				Assert.IsInstanceOf(expectedType, i.Current);
			}
		}

		public static void All(IEnumerable values, IPredicate4 condition)
		{
			IEnumerator iterator = values.GetEnumerator();
			while (iterator.MoveNext())
			{
				if (!condition.Match(iterator.Current))
				{
					Assert.Fail("Condition does not hold for for value '" + iterator.Current + "'.");
				}
			}
		}
	}
}
