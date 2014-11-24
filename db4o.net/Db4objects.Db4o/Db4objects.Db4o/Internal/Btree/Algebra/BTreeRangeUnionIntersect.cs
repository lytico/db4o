/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Btree.Algebra;

namespace Db4objects.Db4o.Internal.Btree.Algebra
{
	/// <exclude></exclude>
	public class BTreeRangeUnionIntersect : BTreeRangeUnionOperation
	{
		public BTreeRangeUnionIntersect(BTreeRangeUnion union) : base(union)
		{
		}

		protected override IBTreeRange Execute(BTreeRangeSingle range)
		{
			return BTreeAlgebra.Intersect(_union, range);
		}

		protected override IBTreeRange Execute(BTreeRangeUnion union)
		{
			return BTreeAlgebra.Intersect(_union, union);
		}
	}
}
