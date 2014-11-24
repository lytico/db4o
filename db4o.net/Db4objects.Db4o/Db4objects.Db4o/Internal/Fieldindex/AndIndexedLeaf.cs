/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
	public class AndIndexedLeaf : JoinedLeaf
	{
		public AndIndexedLeaf(QCon constraint, IIndexedNodeWithRange leaf1, IIndexedNodeWithRange
			 leaf2) : base(constraint, leaf1, leaf1.GetRange().Intersect(leaf2.GetRange()))
		{
		}
	}
}
