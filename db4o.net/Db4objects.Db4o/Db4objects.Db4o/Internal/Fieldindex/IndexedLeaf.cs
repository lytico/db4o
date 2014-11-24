/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	/// <exclude></exclude>
	public class IndexedLeaf : IndexedNodeBase, IIndexedNodeWithRange
	{
		private readonly IBTreeRange _range;

		public IndexedLeaf(QConObject qcon) : base(qcon)
		{
			_range = Search();
		}

		private IBTreeRange Search()
		{
			IBTreeRange range = Search(Constraint().GetObject());
			QEBitmap bitmap = QEBitmap.ForQE(Constraint().Evaluator());
			if (bitmap.TakeGreater())
			{
				if (bitmap.TakeEqual())
				{
					return range.ExtendToLast();
				}
				IBTreeRange greater = range.Greater();
				if (bitmap.TakeSmaller())
				{
					return greater.Union(range.Smaller());
				}
				return greater;
			}
			if (bitmap.TakeSmaller())
			{
				if (bitmap.TakeEqual())
				{
					return range.ExtendToFirst();
				}
				return range.Smaller();
			}
			return range;
		}

		public override int ResultSize()
		{
			return _range.Size();
		}

		public override IEnumerator GetEnumerator()
		{
			return _range.Keys();
		}

		public virtual IBTreeRange GetRange()
		{
			return _range;
		}

		public override void MarkAsBestIndex(QCandidates candidates)
		{
			_constraint.SetProcessedByIndex(candidates);
		}

		public override bool IsEmpty()
		{
			return _range.IsEmpty();
		}
	}
}
