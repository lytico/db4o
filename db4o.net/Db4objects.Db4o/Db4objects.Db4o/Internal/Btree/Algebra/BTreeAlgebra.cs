/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree.Algebra
{
	/// <exclude></exclude>
	internal class BTreeAlgebra
	{
		public static IBTreeRange Intersect(BTreeRangeUnion union, BTreeRangeSingle single
			)
		{
			SortedCollection4 collection = NewBTreeRangeSingleCollection();
			CollectIntersections(collection, union, single);
			return ToRange(collection);
		}

		public static IBTreeRange Intersect(BTreeRangeUnion union1, BTreeRangeUnion union2
			)
		{
			SortedCollection4 collection = NewBTreeRangeSingleCollection();
			IEnumerator ranges = union1.Ranges();
			while (ranges.MoveNext())
			{
				BTreeRangeSingle current = (BTreeRangeSingle)ranges.Current;
				CollectIntersections(collection, union2, current);
			}
			return ToRange(collection);
		}

		private static void CollectIntersections(SortedCollection4 collection, BTreeRangeUnion
			 union, BTreeRangeSingle single)
		{
			IEnumerator ranges = union.Ranges();
			while (ranges.MoveNext())
			{
				BTreeRangeSingle current = (BTreeRangeSingle)ranges.Current;
				if (single.Overlaps(current))
				{
					collection.Add(single.Intersect(current));
				}
			}
		}

		public static IBTreeRange Intersect(BTreeRangeSingle single1, BTreeRangeSingle single2
			)
		{
			BTreePointer first = BTreePointer.Max(single1.First(), single2.First());
			BTreePointer end = BTreePointer.Min(single1.End(), single2.End());
			return single1.NewBTreeRangeSingle(first, end);
		}

		public static IBTreeRange Union(BTreeRangeUnion union1, BTreeRangeUnion union2)
		{
			IEnumerator ranges = union1.Ranges();
			IBTreeRange merged = union2;
			while (ranges.MoveNext())
			{
				merged = merged.Union((IBTreeRange)ranges.Current);
			}
			return merged;
		}

		public static IBTreeRange Union(BTreeRangeUnion union, BTreeRangeSingle single)
		{
			if (single.IsEmpty())
			{
				return union;
			}
			SortedCollection4 sorted = NewBTreeRangeSingleCollection();
			sorted.Add(single);
			BTreeRangeSingle range = single;
			IEnumerator ranges = union.Ranges();
			while (ranges.MoveNext())
			{
				BTreeRangeSingle current = (BTreeRangeSingle)ranges.Current;
				if (CanBeMerged(current, range))
				{
					sorted.Remove(range);
					range = Merge(current, range);
					sorted.Add(range);
				}
				else
				{
					sorted.Add(current);
				}
			}
			return ToRange(sorted);
		}

		private static IBTreeRange ToRange(SortedCollection4 sorted)
		{
			if (1 == sorted.Size())
			{
				return (IBTreeRange)sorted.SingleElement();
			}
			return new BTreeRangeUnion(sorted);
		}

		private static SortedCollection4 NewBTreeRangeSingleCollection()
		{
			return new SortedCollection4(BTreeRangeSingle.Comparison);
		}

		public static IBTreeRange Union(BTreeRangeSingle single1, BTreeRangeSingle single2
			)
		{
			if (single1.IsEmpty())
			{
				return single2;
			}
			if (single2.IsEmpty())
			{
				return single1;
			}
			if (CanBeMerged(single1, single2))
			{
				return Merge(single1, single2);
			}
			return new BTreeRangeUnion(new BTreeRangeSingle[] { single1, single2 });
		}

		private static BTreeRangeSingle Merge(BTreeRangeSingle range1, BTreeRangeSingle range2
			)
		{
			return range1.NewBTreeRangeSingle(BTreePointer.Min(range1.First(), range2.First()
				), BTreePointer.Max(range1.End(), range2.End()));
		}

		private static bool CanBeMerged(BTreeRangeSingle range1, BTreeRangeSingle range2)
		{
			return range1.Overlaps(range2) || range1.Adjacent(range2);
		}
	}
}
