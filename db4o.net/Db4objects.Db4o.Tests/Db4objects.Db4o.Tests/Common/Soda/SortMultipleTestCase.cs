/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class SortMultipleTestCase : AbstractDb4oTestCase
	{
		// COR-18
		public static void Main(string[] arguments)
		{
			new SortMultipleTestCase().RunSolo();
		}

		public class IntHolder
		{
			public int _value;

			public IntHolder(int value)
			{
				this._value = value;
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}
				SortMultipleTestCase.IntHolder intHolder = (SortMultipleTestCase.IntHolder)obj;
				return _value == intHolder._value;
			}

			public override int GetHashCode()
			{
				return _value;
			}

			public override string ToString()
			{
				return _value.ToString();
			}
		}

		public class Data
		{
			public int _first;

			public int _second;

			public SortMultipleTestCase.IntHolder _third;

			public Data(int first, int second, int third)
			{
				this._first = first;
				this._second = second;
				this._third = new SortMultipleTestCase.IntHolder(third);
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}
				SortMultipleTestCase.Data data = (SortMultipleTestCase.Data)obj;
				return _first == data._first && _second == data._second && _third.Equals(data._third
					);
			}

			public override int GetHashCode()
			{
				int hc = _first;
				hc *= 29 + _second;
				hc *= 29 + _third.GetHashCode();
				return hc;
			}

			public override string ToString()
			{
				return _first + "/" + _second + "/" + _third;
			}
		}

		private static readonly SortMultipleTestCase.Data[] TestData = new SortMultipleTestCase.Data
			[] { new SortMultipleTestCase.Data(1, 2, 4), new SortMultipleTestCase.Data(1, 4, 
			3), new SortMultipleTestCase.Data(2, 4, 2), new SortMultipleTestCase.Data(3, 1, 
			4), new SortMultipleTestCase.Data(4, 3, 1), new SortMultipleTestCase.Data(4, 1, 
			3) };

		// 0
		// 1
		// 2
		// 3
		// 4
		// 5
		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int dataIdx = 0; dataIdx < TestData.Length; dataIdx++)
			{
				Store(TestData[dataIdx]);
			}
		}

		public virtual void TestSortFirstThenSecondAfterOr()
		{
			IQuery query = NewQuery(typeof(SortMultipleTestCase.Data));
			query.Descend("_first").Constrain(2).Smaller().Or(query.Descend("_second").Constrain
				(2).Greater());
			query.Descend("_first").OrderAscending();
			query.Descend("_second").OrderAscending();
			AssertSortOrder(query, new int[] { 0, 1, 2, 4 });
		}

		public virtual void TestSortFirstThenSecond()
		{
			IQuery query = NewQuery(typeof(SortMultipleTestCase.Data));
			query.Descend("_first").OrderAscending();
			query.Descend("_second").OrderAscending();
			AssertSortOrder(query, new int[] { 0, 1, 2, 3, 5, 4 });
		}

		public virtual void TestSortSecondThenFirst()
		{
			IQuery query = NewQuery(typeof(SortMultipleTestCase.Data));
			query.Descend("_second").OrderAscending();
			query.Descend("_first").OrderAscending();
			AssertSortOrder(query, new int[] { 3, 5, 0, 4, 1, 2 });
		}

		public virtual void TestSortThirdThenFirst()
		{
			IQuery query = NewQuery(typeof(SortMultipleTestCase.Data));
			query.Descend("_third").Descend("_value").OrderAscending();
			query.Descend("_first").OrderAscending();
			AssertSortOrder(query, new int[] { 4, 2, 1, 5, 0, 3 });
		}

		public virtual void TestSortThirdThenSecond()
		{
			IQuery query = NewQuery(typeof(SortMultipleTestCase.Data));
			query.Descend("_third").Descend("_value").OrderAscending();
			query.Descend("_second").OrderAscending();
			AssertSortOrder(query, new int[] { 4, 2, 5, 1, 3, 0 });
		}

		public virtual void TestSortSecondThenThird()
		{
			IQuery query = NewQuery(typeof(SortMultipleTestCase.Data));
			query.Descend("_second").OrderAscending();
			query.Descend("_third").Descend("_value").OrderAscending();
			AssertSortOrder(query, new int[] { 5, 3, 0, 4, 2, 1 });
		}

		private void AssertSortOrder(IQuery query, int[] expectedIndexes)
		{
			IObjectSet result = query.Execute();
			Assert.AreEqual(expectedIndexes.Length, result.Count);
			for (int i = 0; i < expectedIndexes.Length; i++)
			{
				Assert.AreEqual(TestData[expectedIndexes[i]], result.Next());
			}
		}
	}
}
