/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Btree.Algebra;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeRangeSingle : IBTreeRange
	{
		private sealed class _IComparison4_14 : IComparison4
		{
			public _IComparison4_14()
			{
			}

			public int Compare(object x, object y)
			{
				Db4objects.Db4o.Internal.Btree.BTreeRangeSingle xRange = (Db4objects.Db4o.Internal.Btree.BTreeRangeSingle
					)x;
				Db4objects.Db4o.Internal.Btree.BTreeRangeSingle yRange = (Db4objects.Db4o.Internal.Btree.BTreeRangeSingle
					)y;
				return xRange.First().CompareTo(yRange.First());
			}
		}

		public static readonly IComparison4 Comparison = new _IComparison4_14();

		private readonly Db4objects.Db4o.Internal.Transaction _transaction;

		private readonly BTree _btree;

		private readonly BTreePointer _first;

		private readonly BTreePointer _end;

		public BTreeRangeSingle(Db4objects.Db4o.Internal.Transaction transaction, BTree btree
			, BTreePointer first, BTreePointer end)
		{
			if (transaction == null || btree == null)
			{
				throw new ArgumentNullException();
			}
			_transaction = transaction;
			_btree = btree;
			_first = first;
			_end = end;
		}

		public virtual void Accept(IBTreeRangeVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual bool IsEmpty()
		{
			return BTreePointer.Equals(_first, _end);
		}

		public virtual int Size()
		{
			if (IsEmpty())
			{
				return 0;
			}
			// TODO: This was an attempt to improve size calculation.
			//       Since all nodes are read, there is no improvement.        
			//        BTreeNode currentNode = _first.node();
			//        int sizeOnFirst = currentNode.count() - _first.index();
			//
			//        BTreeNode endNode = _end == null ? null : _end.node();
			//        int substractForEnd = 
			//            (endNode == null) ? 0 : (endNode.count() -  _end.index());
			//        
			//        int size = sizeOnFirst - substractForEnd;
			//        while(! currentNode.equals(endNode)){
			//            currentNode = currentNode.nextNode();
			//            if(currentNode == null){
			//                break;
			//            }
			//            currentNode.prepareRead(transaction());
			//            size += currentNode.count(); 
			//        }
			//        return size;
			int size = 0;
			IEnumerator i = Keys();
			while (i.MoveNext())
			{
				++size;
			}
			return size;
		}

		public virtual IEnumerator Pointers()
		{
			return new BTreeRangePointerIterator(this);
		}

		public virtual IEnumerator Keys()
		{
			return new BTreeRangeKeyIterator(this);
		}

		public BTreePointer End()
		{
			return _end;
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _transaction;
		}

		public virtual BTreePointer First()
		{
			return _first;
		}

		public virtual IBTreeRange Greater()
		{
			return NewBTreeRangeSingle(_end, null);
		}

		public virtual IBTreeRange Union(IBTreeRange other)
		{
			if (null == other)
			{
				throw new ArgumentNullException();
			}
			return new BTreeRangeSingleUnion(this).Dispatch(other);
		}

		public virtual bool Adjacent(Db4objects.Db4o.Internal.Btree.BTreeRangeSingle range
			)
		{
			return BTreePointer.Equals(_end, range._first) || BTreePointer.Equals(range._end, 
				_first);
		}

		public virtual bool Overlaps(Db4objects.Db4o.Internal.Btree.BTreeRangeSingle range
			)
		{
			return FirstOverlaps(this, range) || FirstOverlaps(range, this);
		}

		private bool FirstOverlaps(Db4objects.Db4o.Internal.Btree.BTreeRangeSingle x, Db4objects.Db4o.Internal.Btree.BTreeRangeSingle
			 y)
		{
			return BTreePointer.LessThan(y._first, x._end) && BTreePointer.LessThan(x._first, 
				y._end);
		}

		public virtual IBTreeRange ExtendToFirst()
		{
			return NewBTreeRangeSingle(FirstBTreePointer(), _end);
		}

		public virtual IBTreeRange ExtendToLast()
		{
			return NewBTreeRangeSingle(_first, null);
		}

		public virtual IBTreeRange Smaller()
		{
			return NewBTreeRangeSingle(FirstBTreePointer(), _first);
		}

		public virtual Db4objects.Db4o.Internal.Btree.BTreeRangeSingle NewBTreeRangeSingle
			(BTreePointer first, BTreePointer end)
		{
			return new Db4objects.Db4o.Internal.Btree.BTreeRangeSingle(Transaction(), _btree, 
				first, end);
		}

		public virtual IBTreeRange NewEmptyRange()
		{
			return NewBTreeRangeSingle(null, null);
		}

		private BTreePointer FirstBTreePointer()
		{
			// We don't want nulls included so we have to search for null and use the resulting pointer if we find one.
			IIndexable4 keyHandler = Btree().KeyHandler();
			if (keyHandler is ICanExcludeNullInQueries)
			{
				ICanExcludeNullInQueries canExcludeNullInQueries = (ICanExcludeNullInQueries)keyHandler;
				if (canExcludeNullInQueries.ExcludeNull())
				{
					BTreeNodeSearchResult searchLeaf = Btree().SearchLeafByObject(Transaction(), null
						, SearchTarget.Highest);
					BTreePointer pointer = searchLeaf.FirstValidPointer();
					if (pointer != null)
					{
						return pointer;
					}
				}
			}
			return Btree().FirstPointer(Transaction());
		}

		private BTree Btree()
		{
			return _btree;
		}

		public virtual IBTreeRange Intersect(IBTreeRange range)
		{
			if (null == range)
			{
				throw new ArgumentNullException();
			}
			return new BTreeRangeSingleIntersect(this).Dispatch(range);
		}

		public virtual IBTreeRange ExtendToLastOf(IBTreeRange range)
		{
			Db4objects.Db4o.Internal.Btree.BTreeRangeSingle rangeImpl = CheckRangeArgument(range
				);
			return NewBTreeRangeSingle(_first, rangeImpl._end);
		}

		public override string ToString()
		{
			return "BTreeRangeSingle(first=" + _first + ", end=" + _end + ")";
		}

		private Db4objects.Db4o.Internal.Btree.BTreeRangeSingle CheckRangeArgument(IBTreeRange
			 range)
		{
			if (null == range)
			{
				throw new ArgumentNullException();
			}
			Db4objects.Db4o.Internal.Btree.BTreeRangeSingle rangeImpl = (Db4objects.Db4o.Internal.Btree.BTreeRangeSingle
				)range;
			if (Btree() != rangeImpl.Btree())
			{
				throw new ArgumentException();
			}
			return rangeImpl;
		}

		public virtual BTreePointer LastPointer()
		{
			if (_end == null)
			{
				return Btree().LastPointer(Transaction());
			}
			return _end.Previous();
		}
	}
}
