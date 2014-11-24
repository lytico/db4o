/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class BTreeRangeTestCase : BTreeTestCaseBase
	{
		public static void Main(string[] args)
		{
			new BTreeRangeTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			base.Db4oSetupAfterStore();
			Add(new int[] { 3, 7, 4, 9 });
		}

		public virtual void TestLastPointer()
		{
			AssertLastPointer(8, 7);
			AssertLastPointer(11, 9);
			AssertLastPointer(4, 3);
		}

		private void AssertLastPointer(int searchValue, int expectedValue)
		{
			IBTreeRange single = Search(searchValue);
			IBTreeRange smallerRange = single.Smaller();
			BTreePointer lastPointer = smallerRange.LastPointer();
			Assert.AreEqual(expectedValue, lastPointer.Key());
		}

		public virtual void TestSize()
		{
			AssertSize(4, Range(3, 9));
			AssertSize(3, Range(4, 9));
			AssertSize(3, Range(3, 7));
			AssertSize(4, Range(2, 9));
			AssertSize(4, Range(3, 10));
			Add(new int[] { 5, 6, 8, 10, 2, 1 });
			AssertSize(10, Range(1, 10));
			AssertSize(9, Range(1, 9));
			AssertSize(9, Range(2, 10));
			AssertSize(9, Range(2, 11));
			AssertSize(10, Range(0, 10));
		}

		private void AssertSize(int size, IBTreeRange range)
		{
			Assert.AreEqual(size, range.Size());
		}

		public virtual void TestIntersectSingleSingle()
		{
			AssertIntersection(new int[] { 4, 7 }, Range(3, 7), Range(4, 9));
			AssertIntersection(new int[] {  }, Range(3, 4), Range(7, 9));
			AssertIntersection(new int[] { 3, 4, 7, 9 }, Range(3, 9), Range(3, 9));
			AssertIntersection(new int[] { 3, 4, 7, 9 }, Range(3, 10), Range(3, 9));
			AssertIntersection(new int[] {  }, Range(1, 2), Range(3, 9));
		}

		public virtual void TestIntersectSingleUnion()
		{
			IBTreeRange union = Range(3, 3).Union(Range(7, 9));
			IBTreeRange single = Range(4, 7);
			AssertIntersection(new int[] { 7 }, union, single);
			AssertIntersection(new int[] { 3, 7 }, union, Range(3, 7));
		}

		public virtual void TestIntersectUnionUnion()
		{
			IBTreeRange union1 = Range(3, 3).Union(Range(7, 9));
			IBTreeRange union2 = Range(3, 3).Union(Range(9, 9));
			AssertIntersection(new int[] { 3, 9 }, union1, union2);
		}

		public virtual void TestUnion()
		{
			AssertUnion(new int[] { 3, 4, 7, 9 }, Range(3, 4), Range(7, 9));
			AssertUnion(new int[] { 3, 4, 7, 9 }, Range(3, 7), Range(4, 9));
			AssertUnion(new int[] { 3, 7, 9 }, Range(3, 3), Range(7, 9));
			AssertUnion(new int[] { 3, 9 }, Range(3, 3), Range(9, 9));
		}

		public virtual void TestIsEmpty()
		{
			Assert.IsTrue(Range(0, 0).IsEmpty());
			Assert.IsFalse(Range(3, 3).IsEmpty());
			Assert.IsFalse(Range(9, 9).IsEmpty());
			Assert.IsTrue(Range(10, 10).IsEmpty());
		}

		public virtual void TestUnionWithEmptyDoesNotCreateNewRange()
		{
			IBTreeRange range = Range(3, 4);
			IBTreeRange empty = Range(0, 0);
			Assert.AreSame(range, range.Union(empty));
			Assert.AreSame(range, empty.Union(range));
			IBTreeRange union = range.Union(Range(8, 9));
			Assert.AreSame(union, union.Union(empty));
			Assert.AreSame(union, empty.Union(union));
		}

		public virtual void TestUnionsMerge()
		{
			IBTreeRange range = Range(3, 3).Union(Range(7, 7)).Union(Range(4, 4));
			AssertIsRangeSingle(range);
			BTreeAssert.AssertRange(new int[] { 3, 4, 7 }, range);
		}

		private void AssertIsRangeSingle(IBTreeRange range)
		{
			Assert.IsInstanceOf(typeof(BTreeRangeSingle), range);
		}

		public virtual void TestUnionsOfUnions()
		{
			IBTreeRange union1 = Range(3, 4).Union(Range(8, 9));
			BTreeAssert.AssertRange(new int[] { 3, 4, 9 }, union1);
			BTreeAssert.AssertRange(new int[] { 3, 4, 7, 9 }, union1.Union(Range(7, 7)));
			IBTreeRange union2 = Range(3, 3).Union(Range(7, 7));
			AssertUnion(new int[] { 3, 4, 7, 9 }, union1, union2);
			AssertIsRangeSingle(union1.Union(union2));
			AssertIsRangeSingle(union2.Union(union1));
			IBTreeRange union3 = Range(3, 3).Union(Range(9, 9));
			AssertUnion(new int[] { 3, 7, 9 }, union2, union3);
		}

		public virtual void TestExtendToLastOf()
		{
			BTreeAssert.AssertRange(new int[] { 3, 4, 7 }, Range(3, 7));
			BTreeAssert.AssertRange(new int[] { 4, 7, 9 }, Range(4, 9));
		}

		public virtual void TestUnionOfOverlappingSingleRangesYieldSingleRange()
		{
			Assert.IsInstanceOf(typeof(BTreeRangeSingle), Range(3, 4).Union(Range(4, 9)));
		}

		private void AssertUnion(int[] expectedKeys, IBTreeRange range1, IBTreeRange range2
			)
		{
			BTreeAssert.AssertRange(expectedKeys, range1.Union(range2));
			BTreeAssert.AssertRange(expectedKeys, range2.Union(range1));
		}

		private void AssertIntersection(int[] expectedKeys, IBTreeRange range1, IBTreeRange
			 range2)
		{
			BTreeAssert.AssertRange(expectedKeys, range1.Intersect(range2));
			BTreeAssert.AssertRange(expectedKeys, range2.Intersect(range1));
		}
	}
}
