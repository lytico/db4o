/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class SearcherLowestHighestTestCase : ITestCase, ITestLifeCycle
	{
		private Searcher _searcher;

		private const int SearchFor = 9;

		private static readonly int[] EvenEvenValues = new int[] { 4, 9, 9, 9, 9, 11, 13, 
			17 };

		private static readonly int[] EvenOddValues = new int[] { 4, 5, 9, 9, 9, 11, 13, 
			17 };

		private static readonly int[] OddEvenValues = new int[] { 4, 9, 9, 9, 9, 11, 13 };

		private static readonly int[] OddOddValues = new int[] { 4, 5, 9, 9, 9, 11, 13 };

		private static readonly int[] NoMatchEven = new int[] { 4, 5, 10, 10, 10, 11 };

		private static readonly int[] NoMatchOdd = new int[] { 4, 5, 10, 10, 10, 11, 13 };

		private static readonly int[][] MatchValues = new int[][] { EvenEvenValues, EvenOddValues
			, OddEvenValues, OddOddValues };

		private static readonly int[][] NoMatchValues = new int[][] { NoMatchEven, NoMatchOdd
			 };

		private static readonly SearchTarget[] AllTargets = new SearchTarget[] { SearchTarget
			.Lowest, SearchTarget.Any, SearchTarget.Highest };

		public virtual void TestMatch()
		{
			for (int i = 0; i < MatchValues.Length; i++)
			{
				int[] values = MatchValues[i];
				int lo = LowMatch(values);
				Search(values, SearchTarget.Lowest);
				Assert.AreEqual(lo, _searcher.Cursor());
				Assert.IsTrue(_searcher.FoundMatch());
				int hi = HighMatch(values);
				Search(values, SearchTarget.Highest);
				Assert.AreEqual(hi, _searcher.Cursor());
				Assert.IsTrue(_searcher.FoundMatch());
			}
		}

		public virtual void TestNoMatch()
		{
			for (int i = 0; i < NoMatchValues.Length; i++)
			{
				int[] values = NoMatchValues[i];
				int lo = LowMatch(values);
				Search(values, SearchTarget.Lowest);
				Assert.AreEqual(lo, _searcher.Cursor());
				Assert.IsFalse(_searcher.FoundMatch());
				int hi = HighMatch(values);
				Search(values, SearchTarget.Highest);
				Assert.AreEqual(hi, _searcher.Cursor());
				Assert.IsFalse(_searcher.FoundMatch());
			}
		}

		public virtual void TestEmpty()
		{
			int[] values = new int[] {  };
			for (int i = 0; i < AllTargets.Length; i++)
			{
				Search(values, AllTargets[i]);
				Assert.AreEqual(0, _searcher.Cursor());
				Assert.IsFalse(_searcher.FoundMatch());
				Assert.IsFalse(_searcher.BeforeFirst());
				Assert.IsFalse(_searcher.AfterLast());
			}
		}

		public virtual void TestOneValueMatch()
		{
			int[] values = new int[] { 9 };
			for (int i = 0; i < AllTargets.Length; i++)
			{
				Search(values, AllTargets[i]);
				Assert.AreEqual(0, _searcher.Cursor());
				Assert.IsTrue(_searcher.FoundMatch());
				Assert.IsFalse(_searcher.BeforeFirst());
				Assert.IsFalse(_searcher.AfterLast());
			}
		}

		public virtual void TestOneValueLower()
		{
			int[] values = new int[] { 8 };
			for (int i = 0; i < AllTargets.Length; i++)
			{
				Search(values, AllTargets[i]);
				Assert.AreEqual(0, _searcher.Cursor());
				Assert.IsFalse(_searcher.FoundMatch());
				Assert.IsFalse(_searcher.BeforeFirst());
				Assert.IsTrue(_searcher.AfterLast());
			}
		}

		public virtual void TestOneValueHigher()
		{
			int[] values = new int[] { 8 };
			for (int i = 0; i < AllTargets.Length; i++)
			{
				Search(values, AllTargets[i]);
				Assert.AreEqual(0, _searcher.Cursor());
				Assert.IsFalse(_searcher.FoundMatch());
				Assert.IsFalse(_searcher.BeforeFirst());
				Assert.IsTrue(_searcher.AfterLast());
			}
		}

		public virtual void TestTwoValuesMatch()
		{
			int[] values = new int[] { 9, 9 };
			Search(values, SearchTarget.Lowest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Any);
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Highest);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
		}

		public virtual void TestTwoValuesLowMatch()
		{
			int[] values = new int[] { 9, 10 };
			Search(values, SearchTarget.Lowest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Any);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Highest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
		}

		public virtual void TestTwoValuesHighMatch()
		{
			int[] values = new int[] { 6, 9 };
			Search(values, SearchTarget.Lowest);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Any);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Highest);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsTrue(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
		}

		public virtual void TestTwoValuesInBetween()
		{
			int[] values = new int[] { 8, 10 };
			Search(values, SearchTarget.Lowest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Any);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Highest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
		}

		public virtual void TestTwoValuesLower()
		{
			int[] values = new int[] { 7, 8 };
			Search(values, SearchTarget.Lowest);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsTrue(_searcher.AfterLast());
			Search(values, SearchTarget.Any);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsTrue(_searcher.AfterLast());
			Search(values, SearchTarget.Highest);
			Assert.AreEqual(1, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsFalse(_searcher.BeforeFirst());
			Assert.IsTrue(_searcher.AfterLast());
		}

		public virtual void TestTwoValuesHigher()
		{
			int[] values = new int[] { 10, 11 };
			Search(values, SearchTarget.Lowest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsTrue(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Any);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsTrue(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
			Search(values, SearchTarget.Highest);
			Assert.AreEqual(0, _searcher.Cursor());
			Assert.IsFalse(_searcher.FoundMatch());
			Assert.IsTrue(_searcher.BeforeFirst());
			Assert.IsFalse(_searcher.AfterLast());
		}

		private int Search(int[] values, SearchTarget target)
		{
			_searcher = new Searcher(target, values.Length);
			while (_searcher.Incomplete())
			{
				_searcher.ResultIs(values[_searcher.Cursor()] - SearchFor);
			}
			return _searcher.Cursor();
		}

		private int LowMatch(int[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] == SearchFor)
				{
					return i;
				}
				if (values[i] > SearchFor)
				{
					if (i == 0)
					{
						return 0;
					}
					return i - 1;
				}
			}
			throw new ArgumentException("values");
		}

		private int HighMatch(int[] values)
		{
			for (int i = values.Length - 1; i >= 0; i--)
			{
				if (values[i] <= SearchFor)
				{
					return i;
				}
			}
			throw new ArgumentException("values");
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_searcher = null;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}
	}
}
