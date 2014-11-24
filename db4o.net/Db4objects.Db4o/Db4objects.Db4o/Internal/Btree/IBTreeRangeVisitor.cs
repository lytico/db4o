/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public interface IBTreeRangeVisitor
	{
		void Visit(BTreeRangeSingle range);

		void Visit(BTreeRangeUnion union);
	}
}
