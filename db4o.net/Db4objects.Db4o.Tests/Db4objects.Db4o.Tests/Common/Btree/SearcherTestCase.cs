/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class SearcherTestCase : ITestCase, ITestLifeCycle
	{
		private Searcher _searcher;

		private const int First = 4;

		private const int Last = 11;

		private static readonly int[] EvenValues = new int[] { 4, 7, 9, 11 };

		private static readonly int[] OddValues = new int[] { 4, 7, 8, 9, 11 };

		private static readonly int[] NonMatches = new int[] { 3, 5, 6, 10, 12 };

		private static readonly int[] Matches = new int[] { 4, 7, 9, 11 };

		private const int Before = First - 1;

		private const int Beyond = Last + 1;

		public virtual void TtestPrintResults()
		{
			// not a test, but nice to visualize
			int[] evenValues = new int[] { 4, 7, 9, 11 };
			int[] searches = new int[] { 3, 4, 5, 7, 10, 11, 12 };
			for (int i = 0; i < searches.Length; i++)
			{
				int res = Search(evenValues, searches[i]);
				Sharpen.Runtime.Out.WriteLine(res);
			}
		}

		public virtual void TestCursorEndsOnSmaller()
		{
			Assert.AreEqual(0, Search(EvenValues, 6));
			Assert.AreEqual(0, Search(OddValues, 6));
			Assert.AreEqual(2, Search(EvenValues, 10));
			Assert.AreEqual(3, Search(OddValues, 10));
		}

		public virtual void TestMatchEven()
		{
			AssertMatch(EvenValues);
		}

		public virtual void TestMatchOdd()
		{
			AssertMatch(OddValues);
		}

		public virtual void TestNoMatchEven()
		{
			AssertNoMatch(EvenValues);
		}

		public virtual void TestNoMatchOdd()
		{
			AssertNoMatch(OddValues);
		}

		public virtual void TestBeyondEven()
		{
			AssertBeyond(EvenValues);
		}

		public virtual void TestBeyondOdd()
		{
			AssertBeyond(OddValues);
		}

		public virtual void TestNotBeyondEven()
		{
			AssertNotBeyond(EvenValues);
		}

		public virtual void TestNotBeyondOdd()
		{
			AssertNotBeyond(OddValues);
		}

		public virtual void TestBeforeEven()
		{
			AssertBefore(EvenValues);
		}

		public virtual void TestBeforeOdd()
		{
			AssertBefore(OddValues);
		}

		public virtual void TestNotBeforeEven()
		{
			AssertNotBefore(EvenValues);
		}

		public virtual void TestNotBeforeOdd()
		{
			AssertNotBefore(OddValues);
		}

		public virtual void TestEmptySet()
		{
			_searcher = new Searcher(SearchTarget.Any, 0);
			if (_searcher.Incomplete())
			{
				Assert.Fail();
			}
			Assert.AreEqual(0, _searcher.Cursor());
		}

		private void AssertMatch(int[] values)
		{
			for (int i = 0; i < Matches.Length; i++)
			{
				Search(values, Matches[i]);
				Assert.IsTrue(_searcher.FoundMatch());
			}
		}

		private void AssertNoMatch(int[] values)
		{
			for (int i = 0; i < NonMatches.Length; i++)
			{
				Search(values, NonMatches[i]);
				Assert.IsFalse(_searcher.FoundMatch());
			}
		}

		private void AssertBeyond(int[] values)
		{
			int res = Search(values, Beyond);
			Assert.AreEqual(values.Length - 1, res);
			Assert.IsTrue(_searcher.AfterLast());
		}

		private void AssertNotBeyond(int[] values)
		{
			int res = Search(values, Last);
			Assert.AreEqual(values.Length - 1, res);
			Assert.IsFalse(_searcher.AfterLast());
		}

		private void AssertBefore(int[] values)
		{
			int res = Search(values, Before);
			Assert.AreEqual(0, res);
			Assert.IsTrue(_searcher.BeforeFirst());
		}

		private void AssertNotBefore(int[] values)
		{
			int res = Search(values, First);
			Assert.AreEqual(0, res);
			Assert.IsFalse(_searcher.BeforeFirst());
		}

		private int Search(int[] values, int value)
		{
			_searcher = new Searcher(SearchTarget.Any, values.Length);
			while (_searcher.Incomplete())
			{
				_searcher.ResultIs(values[_searcher.Cursor()] - value);
			}
			return _searcher.Cursor();
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
