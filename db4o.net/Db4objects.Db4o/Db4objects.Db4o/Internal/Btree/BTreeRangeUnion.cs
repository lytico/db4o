/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Btree.Algebra;

namespace Db4objects.Db4o.Internal.Btree
{
	public class BTreeRangeUnion : IBTreeRange
	{
		private readonly BTreeRangeSingle[] _ranges;

		public BTreeRangeUnion(BTreeRangeSingle[] ranges) : this(ToSortedCollection(ranges
			))
		{
		}

		public BTreeRangeUnion(SortedCollection4 sorted)
		{
			if (null == sorted)
			{
				throw new ArgumentNullException();
			}
			_ranges = ToArray(sorted);
		}

		public virtual void Accept(IBTreeRangeVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual bool IsEmpty()
		{
			for (int i = 0; i < _ranges.Length; i++)
			{
				if (!_ranges[i].IsEmpty())
				{
					return false;
				}
			}
			return true;
		}

		private static SortedCollection4 ToSortedCollection(BTreeRangeSingle[] ranges)
		{
			if (null == ranges)
			{
				throw new ArgumentNullException();
			}
			SortedCollection4 collection = new SortedCollection4(BTreeRangeSingle.Comparison);
			for (int i = 0; i < ranges.Length; i++)
			{
				BTreeRangeSingle range = ranges[i];
				if (!range.IsEmpty())
				{
					collection.Add(range);
				}
			}
			return collection;
		}

		private static BTreeRangeSingle[] ToArray(SortedCollection4 collection)
		{
			return (BTreeRangeSingle[])collection.ToArray(new BTreeRangeSingle[collection.Size
				()]);
		}

		public virtual IBTreeRange ExtendToFirst()
		{
			throw new NotImplementedException();
		}

		public virtual IBTreeRange ExtendToLast()
		{
			throw new NotImplementedException();
		}

		public virtual IBTreeRange ExtendToLastOf(IBTreeRange upperRange)
		{
			throw new NotImplementedException();
		}

		public virtual IBTreeRange Greater()
		{
			throw new NotImplementedException();
		}

		public virtual IBTreeRange Intersect(IBTreeRange range)
		{
			if (null == range)
			{
				throw new ArgumentNullException();
			}
			return new BTreeRangeUnionIntersect(this).Dispatch(range);
		}

		public virtual IEnumerator Pointers()
		{
			return Iterators.Concat(Iterators.Map(_ranges, new _IFunction4_77()));
		}

		private sealed class _IFunction4_77 : IFunction4
		{
			public _IFunction4_77()
			{
			}

			public object Apply(object range)
			{
				return ((IBTreeRange)range).Pointers();
			}
		}

		public virtual IEnumerator Keys()
		{
			return Iterators.Concat(Iterators.Map(_ranges, new _IFunction4_85()));
		}

		private sealed class _IFunction4_85 : IFunction4
		{
			public _IFunction4_85()
			{
			}

			public object Apply(object range)
			{
				return ((IBTreeRange)range).Keys();
			}
		}

		public virtual int Size()
		{
			int size = 0;
			for (int i = 0; i < _ranges.Length; i++)
			{
				size += _ranges[i].Size();
			}
			return size;
		}

		public virtual IBTreeRange Smaller()
		{
			throw new NotImplementedException();
		}

		public virtual IBTreeRange Union(IBTreeRange other)
		{
			if (null == other)
			{
				throw new ArgumentNullException();
			}
			return new BTreeRangeUnionUnion(this).Dispatch(other);
		}

		public virtual IEnumerator Ranges()
		{
			return new ArrayIterator4(_ranges);
		}

		public virtual BTreePointer LastPointer()
		{
			throw new NotImplementedException();
		}
	}
}
