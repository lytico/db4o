/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public interface IBTreeStructureListener
	{
		void NotifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode
			);

		void NotifyDeleted(Transaction trans, BTreeNode node);

		void NotifyCountChanged(Transaction trans, BTreeNode node, int diff);
	}
}
