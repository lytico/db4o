/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Data;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Tests.Data
{
	public class GeneratorsTestCase : ITestCase
	{
		public virtual void TestArbitraryIntegerValues()
		{
			CheckArbitraryValuesOf(typeof(int));
		}

		public virtual void TestArbitraryStringValues()
		{
			CheckArbitraryValuesOf(typeof(string));
			Iterator4Assert.All(Generators.ArbitraryValuesOf(typeof(string)), new _IPredicate4_16
				());
		}

		private sealed class _IPredicate4_16 : IPredicate4
		{
			public _IPredicate4_16()
			{
			}

			public bool Match(object candidate)
			{
				return this.IsValidString((string)candidate);
			}

			private bool IsValidString(string s)
			{
				for (int i = 0; i < s.Length; ++i)
				{
					char ch = s[i];
					if (!char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch) && ch != '_')
					{
						return false;
					}
				}
				return true;
			}
		}

		private void CheckArbitraryValuesOf(Type expectedType)
		{
			IEnumerable values = Generators.ArbitraryValuesOf(expectedType);
			Assert.IsTrue(values.GetEnumerator().MoveNext());
			Iterator4Assert.AreInstanceOf(expectedType, values);
		}

		public virtual void TestTake()
		{
			string[] values = new string[] { "1", "2", "3" };
			IEnumerable source = Iterators.Iterable(values);
			AssertTake(new object[0], 0, source);
			AssertTake(new object[] { "1" }, 1, source);
			AssertTake(new object[] { "1", "2" }, 2, source);
			AssertTake(values, 3, source);
			AssertTake(values, 4, source);
		}

		private void AssertTake(object[] expected, int count, IEnumerable source)
		{
			Iterator4Assert.AreEqual(expected, Generators.Take(count, source).GetEnumerator()
				);
		}
	}
}
