/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree.Algebra
{
	/// <exclude></exclude>
	public abstract class BTreeRangeOperation : IBTreeRangeVisitor
	{
		private IBTreeRange _resultingRange;

		public BTreeRangeOperation() : base()
		{
		}

		public virtual IBTreeRange Dispatch(IBTreeRange range)
		{
			range.Accept(this);
			return _resultingRange;
		}

		public void Visit(BTreeRangeSingle single)
		{
			_resultingRange = Execute(single);
		}

		public void Visit(BTreeRangeUnion union)
		{
			_resultingRange = Execute(union);
		}

		protected abstract IBTreeRange Execute(BTreeRangeUnion union);

		protected abstract IBTreeRange Execute(BTreeRangeSingle single);
	}
}
